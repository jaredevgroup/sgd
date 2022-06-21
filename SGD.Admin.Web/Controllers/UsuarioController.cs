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
    [RoutePrefix("usuario")]
    public class UsuarioController : Controller
    {
        [Route("")]
        [Route("index")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("nuevo")]
        [Route("editar/{id}")]
        public ActionResult Mantenimiento(string id = null)
        {
            Route route = RouteData.Route as Route;
            bool esValido = (route.Url == "usuario/nuevo" && string.IsNullOrEmpty(id)) || (route.Url == "usuario/editar/{id}" && !string.IsNullOrEmpty(id));
            if (!esValido) return HttpNotFound();
            Enumeracion.TipoMantenimiento enumTipoMantenimiento = (route.Url == "usuario/nuevo" && string.IsNullOrEmpty(id)) ? Enumeracion.TipoMantenimiento.Nuevo : Enumeracion.TipoMantenimiento.Editar;
            int valorTipoMantenimiento = (int)enumTipoMantenimiento;
            ViewBag.TipoMantenimiento = valorTipoMantenimiento;
            return View("Mantenimiento");
        }
    }
}