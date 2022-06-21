using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BE
{
    public class UsuarioPerfilBE
    {
        public int EntidadDeportivaId { get; set; }
        public string UsuarioId { get; set; }
        public int PerfilId { get; set; }
        public bool FlagActivo { get; set; }
    }
}
