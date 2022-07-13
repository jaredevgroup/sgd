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
    [RoutePrefix("sgd-perfil")]
    public class SGDPerfilController : Controller
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
            bool esValido = (route.Url == "sgd-perfil/nuevo" && id == null) || (route.Url == "sgd-perfil/editar/{id}" && id.HasValue);
            if (!esValido) return HttpNotFound();
            Enumeracion.TipoMantenimiento enumTipoMantenimiento = (route.Url == "sgd-perfil/nuevo" && id == null) ? Enumeracion.TipoMantenimiento.Nuevo : Enumeracion.TipoMantenimiento.Editar;
            int valorTipoMantenimiento = (int)enumTipoMantenimiento;
            ViewBag.TipoMantenimiento = valorTipoMantenimiento;
            return View("Mantenimiento");
        }
    }
}