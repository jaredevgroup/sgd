using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BE
{
    public class PerfilSistemaBE
    {
        public int PerfilId { get; set; }
        public string Nombre { get; set; }
        public bool FlagActivo { get; set; }
        public List<MenuSistemaBE> ListaMenuSistema { get;set; }
    }
}
