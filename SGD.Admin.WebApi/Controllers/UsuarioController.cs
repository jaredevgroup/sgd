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
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {

        UsuarioBL usuarioBL = new UsuarioBL();

        [AuthFilter]
        [HttpGet]
        [Route("buscar-usuario")]
        public IHttpActionResult BuscarUsuario(int entidadDeportivaId, string nombre, string correo, int pageNumber, int pageSize, string sortName, string sortOrder, int draw)
        {
            List<UsuarioBE> lista = usuarioBL.BuscarUsuario(entidadDeportivaId, nombre, correo, pageNumber, pageSize, sortName, sortOrder, out int totalRows, out string mensajeError);

            DataTableCustom<UsuarioBE> result = new DataTableCustom<UsuarioBE> { data = lista, draw = draw, recordsTotal = totalRows, recordsFiltered = totalRows };

            ResponseMessageCustom<DataTableCustom<UsuarioBE>> response = new ResponseMessageCustom<DataTableCustom<UsuarioBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPost]
        [Route("guardar-usuario/{tipoMantenimiento:int}")]
        public IHttpActionResult GuardarUsuario(int tipoMantenimiento, UsuarioBE registro)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seGuardo = usuarioBL.GuardarUsuario(tipoMantenimiento, registro, usuario.UsuarioId, out string mensajeError);

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
        [Route("cambiar-flagactivo-usuario/{entidadDeportivaId:int}/{usuarioId}/{flagActivo:bool}")]
        public IHttpActionResult CambiarFlagActivoUsuario(int entidadDeportivaId, string usuarioId, bool flagActivo)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seCambio = usuarioBL.CambiarFlagActivoUsuario(entidadDeportivaId, usuarioId, !flagActivo, usuario.UsuarioId, out string mensajeError);

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
        [Route("obtener-usuario/{entidadDeportivaId:int}/{usuarioId}")]
        public IHttpActionResult ObtenerUsuario(int entidadDeportivaId, string usuarioId)
        {
            UsuarioBE item = usuarioBL.ObtenerUsuario(entidadDeportivaId, usuarioId, out string mensajeError);

            ResponseMessageCustom<UsuarioBE> response = new ResponseMessageCustom<UsuarioBE>
            {
                status = item == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = item == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = item
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("existe-id-usuario")]
        public IHttpActionResult ExisteIdUsuario(int entidadDeportivaId, int tipoMantenimiento, string usuarioId)
        {
            bool existe = usuarioBL.ExisteIdUsuario(entidadDeportivaId, tipoMantenimiento, usuarioId, out string mensajeError);

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
        [Route("existe-correo-usuario")]
        public IHttpActionResult ExisteCorreoUsuario(int entidadDeportivaId, string usuarioId, string correo)
        {
            bool existe = usuarioBL.ExisteCorreoUsuario(entidadDeportivaId, usuarioId, correo, out string mensajeError);

            ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool>
            {
                status = !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = existe
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPost]
        [Route("guardar-lista-perfil-por-usuario")]
        public IHttpActionResult GuardarListaPerfilPorUsuario(UsuarioBE registro)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seGuardo = usuarioBL.GuardarListaPerfilPorUsuario(registro, usuario.UsuarioId, out string mensajeError);

            ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool>
            {
                status = seGuardo ? "success" : "error",
                message = seGuardo ? "" : mensajeError,
                result = seGuardo
            };

            return Ok(response);
        }
    }
}
