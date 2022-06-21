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
    [RoutePrefix("api/usuariosistema")]
    public class UsuarioSistemaController : ApiController
    {

        UsuarioSistemaBL usuarioBL = new UsuarioSistemaBL();

        [AuthFilter]
        [HttpGet]
        [Route("buscar-usuariosistema")]
        public IHttpActionResult BuscarUsuarioSistema(string nombre, string correo, int pageNumber, int pageSize, string sortName, string sortOrder, int draw)
        {
            List<UsuarioSistemaBE> lista = usuarioBL.BuscarUsuarioSistema(nombre, correo, pageNumber, pageSize, sortName, sortOrder, out int totalRows, out string mensajeError);

            DataTableCustom<UsuarioSistemaBE> result = new DataTableCustom<UsuarioSistemaBE> { data = lista, draw = draw, recordsTotal = totalRows, recordsFiltered = totalRows };

            ResponseMessageCustom<DataTableCustom<UsuarioSistemaBE>> response = new ResponseMessageCustom<DataTableCustom<UsuarioSistemaBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPost]
        [Route("guardar-usuariosistema/{tipoMantenimiento:int}")]
        public IHttpActionResult GuardarUsuarioSistema(int tipoMantenimiento, UsuarioSistemaBE registro)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seGuardo = usuarioBL.GuardarUsuarioSistema(tipoMantenimiento, registro, usuario.UsuarioId, out string mensajeError);

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
        [Route("cambiar-flagactivo-usuariosistema/{usuarioId}/{flagActivo:bool}")]
        public IHttpActionResult CambiarFlagActivoUsuarioSistema(string usuarioId, bool flagActivo)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seCambio = usuarioBL.CambiarFlagActivoUsuarioSistema(usuarioId, !flagActivo, usuario.UsuarioId, out string mensajeError);

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
        [Route("obtener-usuariosistema/{usuarioId}")]
        public IHttpActionResult ObtenerUsuarioSistema(string usuarioId)
        {
            UsuarioSistemaBE item = usuarioBL.ObtenerUsuarioSistema(usuarioId, out string mensajeError);

            ResponseMessageCustom<UsuarioSistemaBE> response = new ResponseMessageCustom<UsuarioSistemaBE>
            {
                status = item == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = item == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = item
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("existe-id-usuariosistema")]
        public IHttpActionResult ExisteIdUsuarioSistema(int tipoMantenimiento, string usuarioId)
        {
            bool existe = usuarioBL.ExisteIdUsuarioSistema(tipoMantenimiento, usuarioId, out string mensajeError);

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
        [Route("existe-correo-usuariosistema")]
        public IHttpActionResult ExisteCorreoUsuarioSistema(string usuarioId, string correo)
        {
            bool existe = usuarioBL.ExisteCorreoUsuarioSistema(usuarioId, correo, out string mensajeError);

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
        [Route("guardar-lista-perfilsistema-por-usuario")]
        public IHttpActionResult GuardarListaPerfilSistemaPorUsuario(UsuarioSistemaBE registro)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seGuardo = usuarioBL.GuardarListaPerfilSistemaPorUsuario(registro, usuario.UsuarioId, out string mensajeError);

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
