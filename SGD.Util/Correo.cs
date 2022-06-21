using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Util
{
    public class Correo
    {
        string correoCredencial = AppSettings.Get<string>("correo.correoCredencial");
        string contraseñaCredencial = AppSettings.Get<string>("correo.contraseñaCredencial");
        string correoSalida = AppSettings.Get<string>("correo.correoSalida");
        string nombreSalida = AppSettings.Get<string>("correo.nombreSalida");
        string hostSmtp = AppSettings.Get<string>("correo.hostSmtp");
        int puertoSmtp = AppSettings.Get<int>("correo.puertoSmtp");

        public bool Enviar(string[] listaCorreoDestinatario, string asunto, string cuerpo, object[][] listaImagenIncrustada = null, string[] listaCorreoCopia = null, string[] listaCorreoCopiaOculta = null, MailPriority prioridad = MailPriority.Normal, bool usarCredencialesPorDefecto = false)
        {
            bool seEnvio = false;
            try
            {
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress(correoSalida, nombreSalida, Encoding.UTF8);//Correo de salida
                if (listaCorreoDestinatario != null)
                {
                    foreach (string item in listaCorreoDestinatario) correo.To.Add(item); //Correo destino?
                }
                if (listaCorreoCopia != null)
                {
                    foreach (string item in listaCorreoCopia) correo.CC.Add(item); //Correo destino?
                }
                if (listaCorreoCopiaOculta != null)
                {
                    foreach (string item in listaCorreoCopiaOculta) correo.Bcc.Add(item); //Correo destino?
                }
                correo.Subject = asunto; //Asunto
                correo.Priority = prioridad;
                correo.IsBodyHtml = true;

                if (listaImagenIncrustada != null)
                {
                    foreach (object[] item in listaImagenIncrustada)
                    {
                        string contentId = (string)item[0];
                        byte[] imagen = (byte[])item[1];

                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(cuerpo, Encoding.UTF8, MediaTypeNames.Text.Html);
                        Stream stream = new MemoryStream(imagen);
                        LinkedResource img = new LinkedResource(stream, MediaTypeNames.Image.Jpeg);
                        img.ContentId = contentId;
                        htmlView.LinkedResources.Add(img);
                        correo.AlternateViews.Add(htmlView);
                    }
                }
                else
                {
                    correo.Body = cuerpo; //Mensaje del correo
                }

                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = usarCredencialesPorDefecto;
                smtp.Host = hostSmtp; //Host del servidor de correo
                smtp.Port = puertoSmtp; //Puerto de salida
                smtp.Credentials = new NetworkCredential(correoCredencial, contraseñaCredencial);//Cuenta de correo
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.EnableSsl = true;//True si el servidor de correo permite ssl
                smtp.Send(correo);
                seEnvio = true;
            }
            catch (Exception ex) { seEnvio = false; }
            
            return seEnvio;
        }
    }
}
