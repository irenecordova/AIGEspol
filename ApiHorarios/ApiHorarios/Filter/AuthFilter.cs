using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHorarios.Filter
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //string key = actionContext.Request.Headers.GetValues("X-SAAC-API-AppKey").FirstOrDefault();
            //string token = actionContext.Request.Headers.GetValues("X-SAAC-API-AppToken").FirstOrDefault();
            base.OnActionExecuting(context);
        }
    }
}
