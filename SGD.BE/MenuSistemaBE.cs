using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BE
{
    public class MenuSistemaBE
    {
        public int MenuId { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
        public string Icono { get; set; }
        public int Orden { get; set; }
        public int? MenuIdPadre { get; set; }
        public bool FlagActivo { get; set; }
        public bool FlagSubMenu { get; set; }
        public List<MenuSistemaBE> ListaSubMenu { get; set; }
    }
}
