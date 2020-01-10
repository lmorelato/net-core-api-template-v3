#pragma warning disable 1998
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Template.Api.Controllers.Bases;
using Template.Api.Extensions.HttpResponse;
using Template.Api.Filters;
using Template.Core.Exceptions;
using Template.Core.Exceptions.Interfaces;

namespace Template.Api.Extensions.ApplicationBuilder
{
    public static partial class ApplicationBuilderExtensions
    {
        // see @ https://www.strathweb.com/2018/07/centralized-exception-handling-and-request-validation-in-asp-net-core/
        public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app, bool isTrusted)
        {
            app.UseExceptionHandler(
                errorApp =>
                {
                    errorApp.Run(
                        async context =>
                        {
                            ProblemDetails problemDetails;
                            var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                            var exception = errorFeature.Error;

                            if (exception is IKnownException)
                            {
                                var modelState = context.Features.Get<ModelStateFeature>()?.ModelState;
                                problemDetails = HandleKnownException(exception, modelState);
                            }
                            else
                            {
                                problemDetails = HandleException(exception, isTrusted);
                            }

                            var json = JsonSerializer.Serialize(problemDetails, problemDetails.GetType());
                            Log.Error(exception, json);

                            context.Response.StatusCode = problemDetails.Status.GetValueOrDefault();
                            context.Response.ContentType = "application/problem+json";
                            await context.Response.WriteAsync(json, Encoding.UTF8);
                        });
                });

            return app;
        }

        private static ProblemDetails HandleKnownException(Exception exception, ModelStateDictionary modelState)
        {
            ProblemDetails problemDetails;

            switch (exception)
            {
                case IdentityResultException ex:
                    modelState ??= new ModelStateDictionary();
                    foreach (var error in ex.Errors)
                    {
                        modelState.TryAddModelError(error.Code, error.Description);
                    }

                    problemDetails = ProblemDetailsFactory.New(HttpStatusCode.BadRequest, modelState, ex);
                    break;

                case NotFoundException ex:
                    problemDetails = ProblemDetailsFactory.New(HttpStatusCode.NotFound, ex);
                    break;

                case EmailNotConfirmedException ex:
                    problemDetails = ProblemDetailsFactory.New(HttpStatusCode.BadRequest, ex);
                    break;

                default:
                    problemDetails = ProblemDetailsFactory.New(HttpStatusCode.BadRequest, exception);
                    break;
            }

            return problemDetails;
        }

        private static ProblemDetails HandleException(Exception exception, bool isTrusted)
        {
            if (exception is BadHttpRequestException badHttpRequestException)
            {
                return HandleBadHttpRequestException(badHttpRequestException, isTrusted);
            }

            return HandleInternalServerError(exception, isTrusted);
        }

        private static ProblemDetails HandleInternalServerError(
            Exception exception,
            bool isTrusted)
        {
            var problemDetails = ProblemDetailsFactory.New(
                HttpStatusCode.InternalServerError,
                isTrusted ? exception.Demystify().ToString() : exception.Message);

            return problemDetails;
        }

        private static ProblemDetails HandleBadHttpRequestException(
            BadHttpRequestException exception,
            bool isTrusted)
        {
            var propertyInfo = typeof(BadHttpRequestException).GetProperty(
                "StatusCode",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var statusCode = (int?)propertyInfo?.GetValue(exception);

            var problemDetails = ProblemDetailsFactory.New(
                statusCode == null ? HttpStatusCode.InternalServerError : (HttpStatusCode)statusCode,
                isTrusted ? exception.Demystify().ToString() : exception.Message);

            return problemDetails;
        }
    }
}
#pragma warning restore 1998