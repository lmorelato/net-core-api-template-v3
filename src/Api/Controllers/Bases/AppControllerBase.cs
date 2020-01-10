using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Template.Api.Controllers.Bases
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class AppControllerBase : ControllerBase
    {
        protected ObjectResultBase CustomResult(HttpStatusCode httpStatusCode)
        {
            return new ObjectResultBase(httpStatusCode);
        }

        protected ObjectResultBase CustomResult(HttpStatusCode httpStatusCode, string message)
        {
            return new ObjectResultBase(httpStatusCode, message);
        }

        protected ObjectResultBase CustomResult(HttpStatusCode httpStatusCode, ModelStateDictionary modelState)
        {
            return new ObjectResultBase(HttpStatusCode.BadRequest, modelState);
        }

        protected new ObjectResultBase BadRequest()
        {
            return new ObjectResultBase(HttpStatusCode.BadRequest);
        }

        protected ObjectResultBase BadRequest(string message)
        {
            return new ObjectResultBase(HttpStatusCode.BadRequest, message);
        }

        protected new ObjectResultBase BadRequest(ModelStateDictionary modelState)
        {
            return new ObjectResultBase(HttpStatusCode.BadRequest, modelState);
        }

        protected new ObjectResultBase NotFound()
        {
            return new ObjectResultBase(HttpStatusCode.NotFound);
        }

        protected ObjectResultBase NotFound(string message)
        {
            return new ObjectResultBase(HttpStatusCode.NotFound, message);
        }
    }
}