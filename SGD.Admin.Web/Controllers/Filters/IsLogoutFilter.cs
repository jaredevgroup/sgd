using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGD.Admin.Web.Controllers.Filters
{
    public class IsLogoutFilter : BaseFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!IsLogin)
            {
                string urlRedirect = filterContext.HttpContext.Request.Url.AbsolutePath;
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "login", Action = "index" }));
                var tempData = filterContext.Controller.TempData;
                tempData["UrlRedirect"] = urlRedirect;
                //filterContext.Controller.ViewBag.UrlRedirect = urlRedirect;
            }
            else
            {
                //int empresaId = Data.Usuario.Empresa.EmpresaId;
                //int usuarioId = Data.Usuario.Id;

                //var empresa = empresaBl.ObtenerEmpresa(empresaId, withUbigeo: true, withConfiguracion: true, columnasEmpresaImagen: new List<enums.Enums.ColumnasEmpresaImagen> { enums.Enums.ColumnasEmpresaImagen.Logo, enums.Enums.ColumnasEmpresaImagen.LogoTipoContenido });

                //filterContext.Controller.ViewBag.Empresa = empresa;
                //filterContext.Controller.ViewBag.Data = Data;
                //filterContext.Controller.ViewBag.ListaPerfil = perfilBl.ListarPerfilPorUsuario(empresaId, usuarioId, loadListaOpcion: true);
                //filterContext.Controller.ViewBag.ListaSede = sedeBl.ListarSedePorUsuario(empresaId, usuarioId);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}