using SGD.Admin.Web.Controllers.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGD.Admin.Web.Controllers
{
    [IsLogoutFilter]
    [RoutePrefix("")]
    public class InicioController : Controller
    {
        [Route("index")]
        [Route("")]
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}