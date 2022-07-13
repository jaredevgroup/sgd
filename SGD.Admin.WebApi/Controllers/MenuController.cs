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
    [RoutePrefix("api/menu")]
    public class MenuController : ApiController
    {
        MenuBL menuBL = new MenuBL();

        [AuthFilter]
        [HttpGet]
        [Route("buscar-menu")]
        public IHttpActionResult BuscarMenu(string nombre, string url, string nombrePadre, int? menuIdPadre, int pageNumber, int pageSize, string sortName, string sortOrder, int draw)
        {
            List<MenuBE> lista = menuBL.BuscarMenu(nombre, url, nombrePadre, menuIdPadre, pageNumber, pageSize, sortName, sortOrder, out int totalRows, out string mensajeError);

            DataTableCustom<MenuBE> result = new DataTableCustom<MenuBE> { data = lista, draw = draw, recordsTotal = totalRows, recordsFiltered = totalRows };

            ResponseMessageCustom<DataTableCustom<MenuBE>> response = new ResponseMessageCustom<DataTableCustom<MenuBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPost]
        [Route("guardar-menu")]
        public IHttpActionResult GuardarMenu(MenuBE registro)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuarioSistema = new UsuarioSistemaBE(cookie.Value);

            bool seGuardo = menuBL.GuardarMenu(registro, usuarioSistema.UsuarioId, out string mensajeError);

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
        [Route("cambiar-flagactivo-menu/{menuId:int}/{flagActivo:bool}")]
        public IHttpActionResult CambiarFlagActivoMenu(int menuId, bool flagActivo)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuarioSistema = new UsuarioSistemaBE(cookie.Value);

            bool seCambio = menuBL.CambiarFlagActivoMenu(menuId, usuarioSistema.UsuarioId, !flagActivo, out string mensajeError);

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
        [Route("obtener-menu/{menuId:int}")]
        public IHttpActionResult ObtenerMenu(int menuId)
        {
            MenuBE item = menuBL.ObtenerMenu(menuId, out string mensajeError);

            ResponseMessageCustom<MenuBE> response = new ResponseMessageCustom<MenuBE>
            {
                status = item == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = item == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = item
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("existe-nombre-menu")]
        public IHttpActionResult ExisteNombreMenu(int? menuId, string nombre, int? menuIdPadre)
        {
            bool existe = menuBL.ExisteNombreMenu(menuId, nombre, menuIdPadre, out string mensajeError);

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
        [Route("listar-menu-por-flagactivo")]
        [Route("listar-menu-por-flagactivo/{flagActivo:bool}")]
        public IHttpActionResult ListarMenuPorFlagActivo(bool? flagActivo = null)
        {
            List<MenuBE> result = menuBL.ListarMenuPorFlagActivo(flagActivo, out string mensajeError);

            ResponseMessageCustom<List<MenuBE>> response = new ResponseMessageCustom<List<MenuBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPut]
        [Route("cambiar-orden-menu/{menuId:int}/{orden:int}")]
        public IHttpActionResult CambiarOrdenMenu(int menuId, int orden)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuarioSistema = new UsuarioSistemaBE(cookie.Value);

            bool seCambio = menuBL.CambiarOrdenMenu(menuId, usuarioSistema.UsuarioId, orden, out string mensajeError);

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
        [Route("listar-menu-por-flagactivo-menuidpadre")]
        [Route("listar-menu-por-flagactivo-menuidpadre/{flagActivo:bool}")]
        [Route("listar-menu-por-flagactivo-menuidpadre/{flagActivo:bool}/{conListaSubMenu:bool}")]
        [Route("listar-menu-por-flagactivo-menuidpadre/{menuIdPadre:int}")]
        [Route("listar-menu-por-flagactivo-menuidpadre/{menuIdPadre:int}/{conListaSubMenu:bool}")]
        [Route("listar-menu-por-flagactivo-menuidpadre/{flagActivo:bool}/{menuIdPadre:int}")]
        [Route("listar-menu-por-flagactivo-menuidpadre/{flagActivo:bool}/{menuIdPadre:int}/{conListaSubMenu:bool}")]
        public IHttpActionResult ListarMenuPorFlagActivoMenuIdPadre(bool? flagActivo = null, int? menuIdPadre = null, bool conListaSubMenu = false)
        {
            List<MenuBE> result = menuBL.ListarMenuPorFlagActivoMenuIdPadre(flagActivo, menuIdPadre, out string mensajeError, conListaSubMenu: conListaSubMenu);

            ResponseMessageCustom<List<MenuBE>> response = new ResponseMessageCustom<List<MenuBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        //[AuthFilter]
        //[HttpGet]
        //[Route("listar-menu-por-flagactivo-perfilid/{perfilId:int}")]
        //[Route("listar-menu-por-flagactivo-perfilid/{perfilId:int}/{flagActivo:bool}")]
        //public IHttpActionResult ListarMenuPorFlagActivoPerfil(int perfilId, bool? flagActivo = null)
        //{
        //    List<MenuBE> result = menuBL.ListarMenuPorFlagActivoPerfil(perfilId, flagActivo, out string mensajeError);

        //    ResponseMessageCustom<List<MenuBE>> response = new ResponseMessageCustom<List<MenuBE>>
        //    {
        //        status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
        //        message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
        //        result = result
        //    };

        //    return Ok(response);
        //}

        [AuthFilter]
        [HttpGet]
        [Route("listar-menu-por-flagactivo-menuidpadre-entidaddeportiva/{entidadDeportivaId:int}")]
        [Route("listar-menu-por-flagactivo-menuidpadre-entidaddeportiva/{entidadDeportivaId:int}/{flagActivo:bool}")]
        [Route("listar-menu-por-flagactivo-menuidpadre-entidaddeportiva/{entidadDeportivaId:int}/{flagActivo:bool}/{conListaSubMenu:bool}")]
        [Route("listar-menu-por-flagactivo-menuidpadre-entidaddeportiva/{entidadDeportivaId:int}/{menuIdPadre:int}")]
        [Route("listar-menu-por-flagactivo-menuidpadre-entidaddeportiva/{entidadDeportivaId:int}/{menuIdPadre:int}/{conListaSubMenu:bool}")]
        [Route("listar-menu-por-flagactivo-menuidpadre-entidaddeportiva/{entidadDeportivaId:int}/{flagActivo:bool}/{menuIdPadre:int}")]
        [Route("listar-menu-por-flagactivo-menuidpadre-entidaddeportiva/{entidadDeportivaId:int}/{flagActivo:bool}/{menuIdPadre:int}/{conListaSubMenu:bool}")]
        public IHttpActionResult ListarMenuPorFlagActivoMenuIdPadreEntidadDeportiva(int entidadDeportivaId, bool? flagActivo = null, int? menuIdPadre = null, bool conListaSubMenu = false)
        {
            List<MenuBE> result = menuBL.ListarMenuPorFlagActivoMenuIdPadreEntidadDeportiva(entidadDeportivaId, flagActivo, menuIdPadre, out string mensajeError, conListaSubMenu: conListaSubMenu);

            ResponseMessageCustom<List<MenuBE>> response = new ResponseMessageCustom<List<MenuBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("listar-menu-por-flagactivo-entidaddeportiva/{entidadDeportivaId:int}")]
        [Route("listar-menu-por-flagactivo-entidaddeportiva/{entidadDeportivaId:int}/{flagActivo:bool}")]
        public IHttpActionResult ListarMenuPorFlagActivoEntidadDeportiva(int entidadDeportivaId, bool? flagActivo = null)
        {
            List<MenuBE> result = menuBL.ListarMenuPorFlagActivoEntidadDeportiva(entidadDeportivaId, flagActivo, out string mensajeError);

            ResponseMessageCustom<List<MenuBE>> response = new ResponseMessageCustom<List<MenuBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("listar-menu-por-flagactivo-entidaddeportivaperfil/{entidadDeportivaId:int}/{perfilId:int}")]
        [Route("listar-menu-por-flagactivo-entidaddeportivaperfil/{entidadDeportivaId:int}/{perfilId:int}/{flagActivo:bool}")]
        public IHttpActionResult ListarMenuPorFlagActivoEntidadDeportivaPerfil(int entidadDeportivaId, int perfilId, bool? flagActivo = null)
        {
            List<MenuBE> result = menuBL.ListarMenuPorFlagActivoEntidadDeportivaPerfil(entidadDeportivaId, perfilId, flagActivo, out string mensajeError);

            ResponseMessageCustom<List<MenuBE>> response = new ResponseMessageCustom<List<MenuBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }
    }
}
