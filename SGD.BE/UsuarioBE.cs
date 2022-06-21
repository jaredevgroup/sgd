using SGD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BE
{
    public class UsuarioBE
    {
        public int EntidadDeportivaId { get; set; }
        public string UsuarioId { get; set; }
        public string Contraseña { get; set; }
        public byte[] ContraseñaByte { get; set; }
        public string Correo { get; set; }
        public bool FlagCambioContraseña { get; set; }
        public bool FlagActivo { get; set; }
        public List<PerfilBE> ListaPerfil { get; set; }
        public List<MenuBE> ListaMenu { get; set; }

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
