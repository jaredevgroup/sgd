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
    [RoutePrefix("api/entidaddeportiva")]
    public class EntidadDeportivaController : ApiController
    {
        EntidadDeportivaBL entidadDeportivaBL = new EntidadDeportivaBL();

        [AuthFilter]
        [HttpGet]
        [Route("buscar-entidaddeportiva")]
        public IHttpActionResult BuscarEntidadDeportiva(string codigo, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, int draw)
        {
            List<EntidadDeportivaBE> lista = entidadDeportivaBL.BuscarEntidadDeportiva(codigo, nombre, pageNumber, pageSize, sortName, sortOrder, out int totalRows, out string mensajeError);

            DataTableCustom<EntidadDeportivaBE> result = new DataTableCustom<EntidadDeportivaBE> { data = lista, draw = draw, recordsTotal = totalRows, recordsFiltered = totalRows };

            ResponseMessageCustom<DataTableCustom<EntidadDeportivaBE>> response = new ResponseMessageCustom<DataTableCustom<EntidadDeportivaBE>>
            {
                status = result == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = result == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = result
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpPost]
        [Route("guardar-entidaddeportiva")]
        public IHttpActionResult GuardarEntidadDeportiva(EntidadDeportivaBE registro)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seGuardo = entidadDeportivaBL.GuardarEntidadDeportiva(registro, usuario.UsuarioId, out string mensajeError);

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
        [Route("cambiar-flagactivo-entidaddeportiva/{entidadDeportivaId:int}/{flagActivo:bool}")]
        public IHttpActionResult CambiarFlagActivoEntidadDeportiva(int entidadDeportivaId, bool flagActivo)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
            UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

            bool seCambio = entidadDeportivaBL.CambiarFlagActivoEntidadDeportiva(entidadDeportivaId, !flagActivo, usuario.UsuarioId, out string mensajeError);

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
        [Route("obtener-entidaddeportiva/{entidadDeportivaId:int}")]
        public IHttpActionResult ObtenerEntidadDeportiva(int entidadDeportivaId)
        {
            EntidadDeportivaBE item = entidadDeportivaBL.ObtenerEntidadDeportiva(entidadDeportivaId, out string mensajeError);

            ResponseMessageCustom<EntidadDeportivaBE> response = new ResponseMessageCustom<EntidadDeportivaBE>
            {
                status = item == null && !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = item == null && !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = item
            };

            return Ok(response);
        }

        [AuthFilter]
        [HttpGet]
        [Route("existe-codigo-entidaddeportiva")]
        public IHttpActionResult ExisteCodigoEntidadDeportiva(int? entidadDeportivaId, string codigo)
        {
            bool existe = entidadDeportivaBL.ExisteCodigoEntidadDeportiva(entidadDeportivaId, codigo, out string mensajeError);

            ResponseMessageCustom<bool> response = new ResponseMessageCustom<bool>
            {
                status = !string.IsNullOrEmpty(mensajeError) ? "error" : "success",
                message = !string.IsNullOrEmpty(mensajeError) ? mensajeError : null,
                result = existe
            };

            return Ok(response);
        }
    }
}
