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
    [RoutePrefix("entidad-deportiva")]
    public class EntidadDeportivaController : Controller
    {
        [Route("")]
        [Route("index")]
        public ActionResult Index()
        {
            return View("Index");
        }

        [Route("nuevo")]
        [Route("editar/{id:int}")]
        public ActionResult Mantenimiento(int? id = null)
        {
            Route route = RouteData.Route as Route;
            bool esValido = (route.Url == "entidad-deportiva/nuevo" && id == null) || (route.Url == "entidad-deportiva/editar/{id}" && id.HasValue);
            if (!esValido) return HttpNotFound();
            Enumeracion.TipoMantenimiento enumTipoMantenimiento = (route.Url == "entidad-deportiva/nuevo" && id == null) ? Enumeracion.TipoMantenimiento.Nuevo : Enumeracion.TipoMantenimiento.Editar;
            int valorTipoMantenimiento = (int)enumTipoMantenimiento;
            ViewBag.TipoMantenimiento = valorTipoMantenimiento;
            return View("Mantenimiento");
        }
    }
}