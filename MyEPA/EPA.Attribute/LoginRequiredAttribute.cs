using System;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.EPA.Attribute
{
    public class LoginRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            HttpSessionStateBase session = actionContext.HttpContext.Session;

            if (session == null || session["AuthenticateId"] == null)
            {
                actionContext.HttpContext.Response.Redirect("~/");

                throw new Exception("Not Login");

            }
            base.OnActionExecuting(actionContext);
        }
    }
}