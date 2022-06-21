using System.Collections.Generic;

namespace SGD.BE
{
    public class MenuBE
    {
        public int MenuId { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
        public string Icono { get; set; }
        public int Orden { get; set; }
        public int? MenuIdPadre { get; set; }
        public bool FlagActivo { get; set; }
        public bool FlagSubMenu { get; set; }
        public List<MenuBE> ListaSubMenu { get; set; }
    }
}