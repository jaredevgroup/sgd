using SGD.Admin.Web.Controllers.Filters;
using SGD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SGD.Admin.Web.Controllers
{
    [IsLogoutFilter]
    [RoutePrefix("menu")]
    public class MenuController : Controller
    {
        [Route("")]
        [Route("index")]
        public ActionResult Index()
        {
            return View("Index");
        }

        [Route("nuevo")]
        [Route("nuevo/{idPadre:int}")]
        [Route("editar/{id:int}")]
        [Route("editar/{id:int}/{idPadre:int}")]
        public ActionResult Mantenimiento(int? id = null, int? idPadre = null)
        {
            Route route = RouteData.Route as Route;
            bool esValido = (route.Url == "menu/nuevo" && !id.HasValue) || (route.Url == "menu/nuevo/{idPadre}" && !id.HasValue && idPadre.HasValue) || (route.Url == "menu/editar/{id}" && id.HasValue && !idPadre.HasValue) || (route.Url == "menu/editar/{id}/{idPadre}" && id.HasValue && idPadre.HasValue);
            if (!esValido) return HttpNotFound();
            Enumeracion.TipoMantenimiento enumTipoMantenimiento = (route.Url == "menu/nuevo" && id == null) ? Enumeracion.TipoMantenimiento.Nuevo : Enumeracion.TipoMantenimiento.Editar;
            int valorTipoMantenimiento = (int)enumTipoMantenimiento;
            ViewBag.TipoMantenimiento = valorTipoMantenimiento;
            return View("Mantenimiento");
        }
    }
}