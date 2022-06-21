using Newtonsoft.Json;
using SGD.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SGD.Admin.Web.Controllers.Filters
{
    public class BaseFilter : ActionFilterAttribute
    {
        protected bool IsLogin
        {
            get
            {
                UsuarioSistemaBE data = null;

                try
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currentSesion");
                    if (cookie != null)
                    {
                        data = new UsuarioSistemaBE(cookie.Value);
                    }
                }
                catch (Exception ex)
                {
                    data = null;
                }

                bool existe = data != null;

                return existe;
            }
        }
    }
}