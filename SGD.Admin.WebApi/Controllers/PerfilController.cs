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
    [RoutePrefix("api/perfil")]
    public class PerfilController : ApiController
    {
        PerfilBL perfilBL = new PerfilBL();

        [AuthFilter]
        [HttpGet]
        [Route("buscar-perfil")]
        public IHttpActionResult BuscarPerfil(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, int draw)
        {
            List<PerfilBE> lista = perfilBL.BuscarPerfil(nombre, pageNumber, pageSize, sortName, sortOrder, out int totalRows, out string mensajeError);

            DataTableCustom<PerfilBE> result = new DataTableCustom<PerfilBE> { data = lista, draw = draw, recordsTotal = totalRows, recordsFiltered = totalRows };

            ResponseMessageCustom<DataTableCustom<PerfilBE>> response = new ResponseMessageCustom<DataTableCustom<PerfilBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPost]
        [Route("guardar-perfil")]
        public IHttpActionResult GuardarPerfil(PerfilBE registro)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seGuardo = perfilBL.GuardarPerfil(registro, usuario.UsuarioId, out string mensajeError);

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
        [Route("cambiar-flagactivo-perfil/{perfilId:int}/{flagActivo:bool}")]
        public IHttpActionResult CambiarFlagActivoPerfil(int perfilId, bool flagActivo)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seCambio = perfilBL.CambiarFlagActivoPerfil(perfilId, usuario.UsuarioId, !flagActivo, out string mensajeError);

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
        [Route("obtener-perfil/{perfilId:int}")]
        public IHttpActionResult ObtenerPerfil(int perfilId)
        {
            PerfilBE item = perfilBL.ObtenerPerfil(perfilId, out string mensajeError);

            ResponseMessageCustom<PerfilBE> response = new ResponseMessageCustom<PerfilBE>
            {
                status = item == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = item == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = item
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("existe-nombre-perfil")]
        public IHttpActionResult ExisteNombrePerfil(int? perfilId, string nombre)
        {
            bool existe = perfilBL.ExisteNombrePerfil(perfilId, nombre, out string mensajeError);

            ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool>
            {
                status = !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = existe
            };

            return Ok(response);
        }

        //[AuthFilter]
        //[HttpPost]
        //[Route("guardar-lista-menu-por-perfil")]
        //public IHttpActionResult GuardarListaMenuPorPerfil(PerfilBE registro)
        //{
        //    HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
        //    UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

        //    bool seGuardo = perfilBL.GuardarListaMenuPorPerfil(registro, usuario.UsuarioId, out string mensajeError);

        //    ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool>
        //    {
        //        status = seGuardo ? "success" : "error",
        //        message = seGuardo ? "" : mensajeError,
        //        result = seGuardo
        //    };

        //    return Ok(response);
        //}

        [AuthFilter]
        [HttpGet]
        [Route("listar-perfil-por-flagactivo")]
        [Route("listar-perfil-por-flagactivo/{flagActivo:bool}")]
        public IHttpActionResult ListarPerfilPorFlagActivo(bool? flagActivo = null)
        {
            List<PerfilBE> result = perfilBL.ListarPerfilPorFlagActivo(flagActivo, out string mensajeError);

            ResponseMessageCustom<List<PerfilBE>> response = new ResponseMessageCustom<List<PerfilBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("listar-perfil-por-flagactivo-usuario/{entidadDeportivaId:int}/{usuarioId}")]
        [Route("listar-perfil-por-flagactivo-usuario/{entidadDeportivaId:int}/{usuarioId}/{flagActivo:bool}")]
        public IHttpActionResult ListarPerfilPorFlagActivoUsuario(int entidadDeportivaId, string usuarioId, bool? flagActivo = null)
        {
            List<PerfilBE> result = perfilBL.ListarPerfilPorFlagActivoUsuario(entidadDeportivaId, usuarioId, flagActivo, out string mensajeError);

            ResponseMessageCustom<List<PerfilBE>> response = new ResponseMessageCustom<List<PerfilBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("listar-perfil-por-flagactivo-entidaddeportiva/{entidadDeportivaId:int}")]
        [Route("listar-perfil-por-flagactivo-entidaddeportiva/{entidadDeportivaId:int}/{flagActivo:bool}")]
        public IHttpActionResult ListarPerfilPorFlagActivoEntidadDeportiva(int entidadDeportivaId, bool? flagActivo = null)
        {
            List<PerfilBE> result = perfilBL.ListarPerfilPorFlagActivoEntidadDeportiva(entidadDeportivaId, flagActivo, out string mensajeError);

            ResponseMessageCustom<List<PerfilBE>> response = new ResponseMessageCustom<List<PerfilBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }
    }
}
