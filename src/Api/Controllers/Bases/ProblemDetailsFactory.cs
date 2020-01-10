using System;
using System.Linq;
using System.Net;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Template.Api.Controllers.Bases
{
    public static class ProblemDetailsFactory
    {
        public static ProblemDetails New(HttpStatusCode httpStatusCode)
        {
            var problemDetails = new ProblemDetails();
            SetValuesByDetail(problemDetails, httpStatusCode, null);
            return problemDetails;
        }

        public static ProblemDetails New(HttpStatusCode httpStatusCode, string detail)
        {
            var problemDetails = new ProblemDetails();
            SetValuesByDetail(problemDetails, httpStatusCode, detail);
            return problemDetails;
        }

        public static ProblemDetails New(HttpStatusCode httpStatusCode, Exception exception)
        {
            var problemDetails = new ProblemDetails();
            SetValuesByException(problemDetails, httpStatusCode, exception);
            return problemDetails;
        }

        public static ProblemDetails New(HttpStatusCode httpStatusCode, ModelStateDictionary modelState)
        {
            var hasError = modelState != null && !modelState.IsValid;
            var problemDetails = hasError ?
                                     new ValidationProblemDetails(modelState) :
                                     new ProblemDetails();

            SetValuesByDetail(problemDetails, httpStatusCode, GetProblemDetailDescription(modelState));
            return problemDetails;
        }

        public static ProblemDetails New(HttpStatusCode httpStatusCode, ModelStateDictionary modelState, Exception exception)
        {
            var hasError = modelState != null && !modelState.IsValid;
            var problemDetails = hasError ?
                                     new ValidationProblemDetails(modelState) :
                                     new ProblemDetails();

            SetValuesByDetailAndException(problemDetails, httpStatusCode, GetProblemDetailDescription(modelState), exception);
            return problemDetails;
        }

        private static void SetValuesByDetail(ProblemDetails problemDetails, HttpStatusCode httpStatusCode, string detail)
        {
            SetCommonValues(problemDetails, httpStatusCode);
            problemDetails.Detail = detail;
            problemDetails.Instance = $"urn:api:{httpStatusCode.ToString().ToLower()}:{Guid.NewGuid()}";
        }

        private static void SetValuesByException(ProblemDetails problemDetails, HttpStatusCode httpStatusCode, Exception exception)
        {
            SetCommonValues(problemDetails, httpStatusCode);
            problemDetails.Detail = exception.Message;

            problemDetails.Instance = "urn:api:{0}:{1}:{2}".FormatWith(
                httpStatusCode.ToString().ToLower(),
                exception.GetType().Name.ToLower(),
                Guid.NewGuid());
        }

        private static void SetValuesByDetailAndException(ProblemDetails problemDetails, HttpStatusCode httpStatusCode, string detail, Exception exception)
        {
            SetCommonValues(problemDetails, httpStatusCode);
            problemDetails.Detail = detail;
            problemDetails.Instance = "urn:api:{0}:{1}:{2}".FormatWith(
                httpStatusCode.ToString().ToLower(),
                exception.GetType().Name.ToLower(),
                Guid.NewGuid());
        }

        private static void SetCommonValues(ProblemDetails problemDetails, HttpStatusCode httpStatusCode)
        {
            problemDetails.Status = (int)httpStatusCode;
            problemDetails.Title = httpStatusCode.ToString().Titleize();
        }

        private static string GetProblemDetailDescription(ModelStateDictionary modelState)
        {
            if (modelState == null || modelState.IsValid)
            {
                return null;
            }

            var errorEntries = modelState.Where(e => e.Value.Errors.Count > 0).ToList();
            if (errorEntries.Count == 1 &&
                errorEntries.First().Value.Errors.Count == 1 &&
                errorEntries.First().Key == string.Empty)
            {
                return errorEntries.First().Value.Errors.First().ErrorMessage;
            }

            return "See errors for details";
        }
    }
}
