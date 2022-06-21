using Newtonsoft.Json;
using SGD.BE;
using SGD.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SGD.Admin.WebApi.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthFilter : AuthorizationFilterAttribute
    {
        UsuarioSistemaBL usuarioSistemaBL = new UsuarioSistemaBL();

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
                if(cookie == null)
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    return;
                }

                UsuarioSistemaBE usuario = new UsuarioSistemaBE(cookie.Value);

                //usuario = usuarioSistemaBL.ObtenerUsuarioSistema(usuario.UsuarioId, out string mensajeError);

                if(usuario == null)
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    return;
                }

                //bool esValido = usuarioSistemaBL.ValidarUsuario(usuario.UsuarioId, usuario.ContraseñaByte, out usuario, out string mensajeError);

                //if (!esValido)
                //{
                //    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                //    return;
                //}
            }
            catch (Exception)
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }

            base.OnAuthorization(filterContext);
        }
    }
}