using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BE
{
    public class EntidadDeportivaBE
    {
        public int EntidadDeportivaId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool FlagActivo { get; set; }
    }
}
