using System.Net;

using Microsoft.AspNetCore.Mvc.Filters;

using Template.Api.Controllers.Bases;

namespace Template.Api.Filters
{
    public class ModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ObjectResultBase(HttpStatusCode.BadRequest, context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Features.Set(new ModelStateFeature(context.ModelState));
        }
    }
}
