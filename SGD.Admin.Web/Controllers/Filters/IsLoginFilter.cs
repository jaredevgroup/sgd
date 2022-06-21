using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGD.Admin.Web.Controllers.Filters
{
    public class IsLoginFilter : BaseFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsLogin)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "inicio", Action = "index" }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}