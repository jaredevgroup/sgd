using Newtonsoft.Json;
using SGD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BE
{
    public class UsuarioSistemaBE
    {
        public UsuarioSistemaBE() { }
        public UsuarioSistemaBE(string jsonStringCodificado)
        {
            string cookieStringDescodificado = Encoding.UTF8.GetString(Convert.FromBase64String(jsonStringCodificado));
            UsuarioSistemaBE usuario = JsonConvert.DeserializeObject<UsuarioSistemaBE>(cookieStringDescodificado);
            if (usuario == null) return;
            PropertyInfo[] propertyInfos = usuario.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                object value = propertyInfo.GetValue(usuario);
                propertyInfo.SetValue(this, value);
            }
        }

        public string UsuarioId { get; set; }
        public string Contraseña { get; set; }
        public byte[] ContraseñaByte { get; set; }
        public string Correo { get; set; }
        public bool FlagCambioContraseña { get; set; }
        public bool FlagActivo { get; set; }
        public List<PerfilSistemaBE> ListaPerfilSistema { get; set; }
        public List<MenuSistemaBE> ListaMenuSistema { get; set; }

        #region Métodos
        public byte[] ObtenerContraseñaCodificada()
        {
            byte[] valor = null;

            if (!string.IsNullOrEmpty(Contraseña)) valor = Seguridad.MD5.Encriptar(Contraseña);

            return valor;
        }
        #endregion
    }
}
