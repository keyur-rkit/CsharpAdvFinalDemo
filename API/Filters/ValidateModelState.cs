using Mysqlx;
using System;
using System.Net.Http;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.Filters
{
    public class ValidateModelState : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
               actionContext.Response = actionContext.Request.CreateResponse(
               HttpStatusCode.BadRequest,
               new
               {
                   actionContext.ModelState
               });
            }
        }
    }
}