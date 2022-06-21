using Newtonsoft.Json;
using SGD.Admin.Web.Controllers.Filters;
using SGD.BE;
using SGD.BL;
using SGD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SGD.Admin.Web.Controllers
{
    [RoutePrefix("login")]
    public class LoginController : Controller
    {
        UsuarioSistemaBL usuarioSistemaBL = new UsuarioSistemaBL();
        TokenBL tokenBl = new TokenBL();
        int sesionMinutes = AppSettings.Get<int>("sesion.minutes");

        [IsLoginFilter]
        [Route("")]
        [Route("index")]
        public ActionResult Index()
        {
            return View("Index");
        }

        [IsLoginFilter]
        [Route("recuperar-contraseña")]
        public ActionResult RecuperarContraseña()
        {
            return View("RecuperarContraseña");
        }

        [IsLoginFilter]
        [Route("cambiar-contraseña/{tokenId}/{usuarioId}")]
        public ActionResult CambiarContraseña(string tokenId, string usuarioId)
        {
            string usuarioIdDesencriptado = Encoding.UTF8.GetString(Convert.FromBase64String(usuarioId));
            bool esValido = tokenBl.ValidarTokenPorUsuarioSistema(tokenId, usuarioIdDesencriptado, out string mensajeError);

            if (esValido) TempData["login-message-error"] = mensajeError ?? "Ocurrió un error";

            return View("CambiarContraseña");
        }

        [HttpPost]
        [Route("autenticar")]
        public ActionResult Autenticar(string usuario, string contraseña)
        {
            bool esValido = usuarioSistemaBL.ValidarUsuario(usuario, contraseña, out UsuarioSistemaBE usuarioSistema, out string mensajeError);

            if (!esValido)
            {
                TempData["login-message-error"] = mensajeError ?? "No se pudo validar las credenciales";
                return RedirectToAction("index", "login");
            }

            string usuarioSistemaJsonString = JsonConvert.SerializeObject(usuarioSistema);
            string usuarioSistemaJsonStringCodificado = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuarioSistemaJsonString));
            HttpCookie cookieSesion = new HttpCookie("currentSesion", usuarioSistemaJsonStringCodificado);
            string cookieDomain = AppSettings.Get<string>("cookie.domain");
            cookieSesion.Domain = cookieDomain;
            cookieSesion.Expires = DateTime.Now.AddMinutes(sesionMinutes);
            HttpContext.Response.Cookies.Add(cookieSesion);
            return RedirectToAction("index", "inicio");
        }

        [HttpPost]
        [Route("enviar-correo-recuperacion-contraseña")]
        public ActionResult EnviarCorreoRecuperacionContraseña(string correo)
        {
            string urlBase = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            bool seEnvioCorreo = usuarioSistemaBL.EnviarCorreoRecuperacionContraseña(correo, urlBase, out string mensajeError);

            if (!seEnvioCorreo) TempData["login-message-error"] = mensajeError ?? "No se pudo enviar el correo";
            else TempData["login-message-success"] = $"Se envío el correo de recuperación a {correo.ToLower()}";

            return RedirectToAction("index", "login");
        }

        [HttpPost]
        [Route("actualizar-contraseña")]
        public ActionResult ActualizarContraseña(string contraseña, string usuarioId)
        {
            string usuarioIdDesencriptado = Encoding.UTF8.GetString(Convert.FromBase64String(usuarioId));

            bool seActualizo = usuarioSistemaBL.ActualizarContraseña(usuarioIdDesencriptado, contraseña, out string mensajeError);

            if (!seActualizo)
            {
                TempData["login-message-error"] = mensajeError;
                return RedirectToAction("index", "login");
            }

            return Autenticar(usuarioIdDesencriptado, contraseña);
        }

        [IsLogoutFilter]
        [Route("salir")]
        public ActionResult Salir()
        {
            HttpCookie cookieCurrentSesionResponse = System.Web.HttpContext.Current.Response.Cookies.Get("currentSesion");
            if (cookieCurrentSesionResponse != null)
            {
                string cookieDomain = AppSettings.Get<string>("cookie.domain");
                cookieCurrentSesionResponse.Domain = cookieDomain;
                cookieCurrentSesionResponse.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies.Set(cookieCurrentSesionResponse);
            }

            //HttpCookie cookieCurrentSesionRequest = System.Web.HttpContext.Current.Request.Cookies.Get("currentSesion");
            //if (cookieCurrentSesionRequest != null)
            //{
            //    mensaje = $"{mensaje}<br><br>Fecha caducidad actual: {cookieCurrentSesionRequest.Expires}";
            //    cookieCurrentSesionRequest.Expires = DateTime.Now.AddDays(-1);
            //    System.Web.HttpContext.Current.Request.Cookies.Set(cookieCurrentSesionRequest);
            //    mensaje = $"{mensaje}<br>Fecha caducidad cambiado: {cookieCurrentSesionRequest.Expires}, Dominio: {cookieCurrentSesionRequest.Domain}";

            //}
            //return Content($"Se encontró {mensaje}");

            return RedirectToAction("index", "login");
        }
    }
}