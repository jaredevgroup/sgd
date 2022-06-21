using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SGD.Util.Enumeracion;

namespace SGD.BE
{
    public class TokenBE
    {
        public string TokenId { get; set; }
        public TipoToken EnumTipoToken { get; set; }
        public DateTime FechaCreacion { get; set; }
        public long MilisegundosDuracion { get; set; }

        public TokenUsuarioSistemaBE TokenUsuarioSistema { get; set; }
        public TokenUsuarioBE TokenUsuario { get; set; }

        #region Métodos
        public DateTime ObtenerFechaCaducidad()
        {
            DateTime valor = FechaCreacion.AddMilliseconds(MilisegundosDuracion);
            return valor;
        }
        public int ObtenerValorEnumTipoToken()
        {
            int valorTipoToken = (int)EnumTipoToken;
            return valorTipoToken;
        }
        #endregion
    }
}
