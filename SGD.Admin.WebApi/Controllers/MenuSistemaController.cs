using SGD.Admin.WebApi.Controllers.Filters;
using SGD.Admin.WebApi.Models;
using SGD.BE;
using SGD.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SGD.Admin.WebApi.Controllers
{
    [RoutePrefix("api/menusistema")]
    public class MenuSistemaController : ApiController
    {
        MenuSistemaBL menuBL = new MenuSistemaBL();

        [AuthFilter]
        [HttpGet]
        [Route("buscar-menusistema")]
        public IHttpActionResult BuscarMenuSistema(string nombre, string url, string nombrePadre, int? menuIdPadre, int pageNumber, int pageSize, string sortName, string sortOrder, int draw)
        {
            List<MenuSistemaBE> lista = menuBL.BuscarMenuSistema(nombre, url, nombrePadre, menuIdPadre, pageNumber, pageSize, sortName, sortOrder, out int totalRows, out string mensajeError);

            DataTableCustom<MenuSistemaBE> result = new DataTableCustom<MenuSistemaBE> { data = lista, draw = draw, recordsTotal = totalRows, recordsFiltered = totalRows };

            ResponseMessageCustom<DataTableCustom<MenuSistemaBE>> response = new ResponseMessageCustom<DataTableCustom<MenuSistemaBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPost]
        [Route("guardar-menusistema")]
        public IHttpActionResult GuardarMenuSistema(MenuSistemaBE registro)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seGuardo = menuBL.GuardarMenuSistema(registro, usuario.UsuarioId, out string mensajeError);

            ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool>
            {
                status = seGuardo ? "success" : "error",
                message = seGuardo ? "" : mensajeError,
                result = seGuardo
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPut]
        [Route("cambiar-flagactivo-menusistema/{menuId:int}/{flagActivo:bool}")]
        public IHttpActionResult CambiarFlagActivoMenuSistema(int menuId, bool flagActivo)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seCambio = menuBL.CambiarFlagActivoMenuSistema(menuId, usuario.UsuarioId, !flagActivo, out string mensajeError);

            ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool>
            {
                status = seCambio ? "success" : "error",
                message = seCambio ? "" : mensajeError,
                result = seCambio
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("obtener-menusistema/{menuId:int}")]
        public IHttpActionResult ObtenerMenuSistema(int menuId)
        {
            MenuSistemaBE item = menuBL.ObtenerMenuSistema(menuId, out string mensajeError);

            ResponseMessageCustom<MenuSistemaBE> response = new ResponseMessageCustom<MenuSistemaBE>
            {
                status = item == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = item == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = item
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("existe-nombre-menusistema")]
        public IHttpActionResult ExisteNombreMenuSistema(int? menuId, string nombre, int? menuIdPadre)
        {
            bool existe = menuBL.ExisteNombreMenuSistema(menuId, nombre, menuIdPadre, out string mensajeError);

            ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool>
            {
                status = !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = existe
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("listar-menusistema-por-flagactivo")]
        [Route("listar-menusistema-por-flagactivo/{flagActivo:bool}")]
        public IHttpActionResult ListarMenuSistemaPorFlagActivo(bool? flagActivo = null)
        {
            List<MenuSistemaBE> result = menuBL.ListarMenuSistemaPorFlagActivo(flagActivo, out string mensajeError);

            ResponseMessageCustom<List<MenuSistemaBE>> response = new ResponseMessageCustom<List<MenuSistemaBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPut]
        [Route("cambiar-orden-menusistema/{menuId:int}/{orden:int}")]
        public IHttpActionResult CambiarOrdenMenuSistema(int menuId, int orden)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seCambio = menuBL.CambiarOrdenMenuSistema(menuId, usuario.UsuarioId, orden, out string mensajeError);

            ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool>
            {
                status = seCambio ? "success" : "error",
                message = seCambio ? "" : mensajeError,
                result = seCambio
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("listar-menusistema-por-flagactivo-menuidpadre")]
        [Route("listar-menusistema-por-flagactivo-menuidpadre/{flagActivo:bool}")]
        [Route("listar-menusistema-por-flagactivo-menuidpadre/{flagActivo:bool}/{conListaSubMenu:bool}")]
        [Route("listar-menusistema-por-flagactivo-menuidpadre/{menuIdPadre:int}")]
        [Route("listar-menusistema-por-flagactivo-menuidpadre/{menuIdPadre:int}/{conListaSubMenu:bool}")]
        [Route("listar-menusistema-por-flagactivo-menuidpadre/{flagActivo:bool}/{menuIdPadre:int}")]
        [Route("listar-menusistema-por-flagactivo-menuidpadre/{flagActivo:bool}/{menuIdPadre:int}/{conListaSubMenu:bool}")]
        public IHttpActionResult ListarMenuSistemaPorFlagActivoMenuIdPadre(bool? flagActivo = null, int? menuIdPadre = null, bool conListaSubMenu = false)
        {
            List<MenuSistemaBE> result = menuBL.ListarMenuSistemaPorFlagActivoMenuIdPadre(flagActivo, menuIdPadre, out string mensajeError, conListaSubMenu: conListaSubMenu);

            ResponseMessageCustom<List<MenuSistemaBE>> response = new ResponseMessageCustom<List<MenuSistemaBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("listar-menusistema-por-flagactivo-perfilid/{perfilId:int}")]
        [Route("listar-menusistema-por-flagactivo-perfilid/{perfilId:int}/{flagActivo:bool}")]
        public IHttpActionResult ListarMenuSistemaPorFlagActivoPerfil(int perfilId, bool? flagActivo = null)
        {
            List<MenuSistemaBE> result = menuBL.ListarMenuSistemaPorFlagActivoPerfil(perfilId, flagActivo, out string mensajeError);

            ResponseMessageCustom<List<MenuSistemaBE>> response = new ResponseMessageCustom<List<MenuSistemaBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }
    }
}
