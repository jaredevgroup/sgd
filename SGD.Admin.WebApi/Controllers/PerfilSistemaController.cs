using Newtonsoft.Json;
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
    [RoutePrefix("api/perfilsistema")]
    public class PerfilSistemaController : ApiController
    {
        PerfilSistemaBL perfilBL = new PerfilSistemaBL();

        [AuthFilter]
        [HttpGet]
        [Route("buscar-perfilsistema")]
        public IHttpActionResult BuscarPerfilSistema(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, int draw)
        {
            List<PerfilSistemaBE> lista = perfilBL.BuscarPerfilSistema(nombre, pageNumber, pageSize, sortName, sortOrder, out int totalRows, out string mensajeError);

            DataTableCustom<PerfilSistemaBE> result = new DataTableCustom<PerfilSistemaBE> { data = lista, draw = draw, recordsTotal = totalRows, recordsFiltered = totalRows };

            ResponseMessageCustom<DataTableCustom<PerfilSistemaBE>> response = new ResponseMessageCustom<DataTableCustom<PerfilSistemaBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPost]
        [Route("guardar-perfilsistema")]
        public IHttpActionResult GuardarPerfilSistema(PerfilSistemaBE registro)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seGuardo = perfilBL.GuardarPerfilSistema(registro, usuario.UsuarioId, out string mensajeError);

            ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool> { 
                status = seGuardo ? "success" : "error",
                message = seGuardo ? "" : mensajeError,
                result = seGuardo
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPut]
        [Route("cambiar-flagactivo-perfilsistema/{perfilId:int}/{flagActivo:bool}")]
        public IHttpActionResult CambiarFlagActivoPerfilSistema(int perfilId, bool flagActivo)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seCambio = perfilBL.CambiarFlagActivoPerfilSistema(perfilId, usuario.UsuarioId, !flagActivo, out string mensajeError);

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
        [Route("obtener-perfilsistema/{perfilId:int}")]
        public IHttpActionResult ObtenerPerfilSistema(int perfilId)
        {
            PerfilSistemaBE item = perfilBL.ObtenerPerfilSistema(perfilId, out string mensajeError);

            ResponseMessageCustom<PerfilSistemaBE> response = new ResponseMessageCustom<PerfilSistemaBE>
            {
                status = item == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = item == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = item
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("existe-nombre-perfilsistema")]
        public IHttpActionResult ExisteNombrePerfilSistema(int? perfilId, string nombre)
        {
            bool existe = perfilBL.ExisteNombrePerfilSistema(perfilId, nombre, out string mensajeError);

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
        [Route("guardar-lista-menusistema-por-perfil")]
        public IHttpActionResult GuardarListaMenuSistemaPorPerfil(PerfilSistemaBE registro)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seGuardo = perfilBL.GuardarListaMenuSistemaPorPerfil(registro, usuario.UsuarioId, out string mensajeError);

            ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool>
            {
                status = seGuardo ? "success" : "error",
                message = seGuardo ? "" : mensajeError,
                result = seGuardo
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("listar-perfilsistema-por-flagactivo")]
        [Route("listar-perfilsistema-por-flagactivo/{flagActivo:bool}")]
        public IHttpActionResult ListarPerfilSistemaPorFlagActivo(bool? flagActivo = null)
        {
            List<PerfilSistemaBE> result = perfilBL.ListarPerfilSistemaPorFlagActivo(flagActivo, out string mensajeError);

            ResponseMessageCustom<List<PerfilSistemaBE>> response = new ResponseMessageCustom<List<PerfilSistemaBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("listar-perfilsistema-por-flagactivo-usuarioid/{usuarioId}")]
        [Route("listar-perfilsistema-por-flagactivo-usuarioid/{usuarioId}/{flagActivo:bool}")]
        public IHttpActionResult ListarPerfilSistemaPorFlagActivoUsuario(string usuarioId, bool? flagActivo = null)
        {
            List<PerfilSistemaBE> result = perfilBL.ListarPerfilSistemaPorFlagActivoUsuario(usuarioId, flagActivo, out string mensajeError);

            ResponseMessageCustom<List<PerfilSistemaBE>> response = new ResponseMessageCustom<List<PerfilSistemaBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }
    }
}
