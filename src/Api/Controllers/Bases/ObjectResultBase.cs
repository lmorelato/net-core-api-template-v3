using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Template.Api.Controllers.Bases
{
    // see @ https://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi/
    public class ObjectResultBase : ObjectResult
    {
        public ObjectResultBase(HttpStatusCode httpStatus) : base(null)
        {
            this.StatusCode = (int)httpStatus;
            this.Value = ProblemDetailsFactory.New(httpStatus);
        }

        public ObjectResultBase(HttpStatusCode httpStatus, string message) : base(null)
        {
            this.StatusCode = (int)httpStatus;
            this.Value = ProblemDetailsFactory.New(httpStatus, message);
        }

        public ObjectResultBase(HttpStatusCode httpStatus, ModelStateDictionary modelState) : base(null)
        {
            this.StatusCode = (int)httpStatus;
            this.Value = ProblemDetailsFactory.New(httpStatus, modelState); 
        }
    }
}
