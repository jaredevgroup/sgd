using System.Collections.Generic;

namespace SGD.BE
{
    public class PerfilBE
    {
        public int PerfilId { get; set; }
        public string Nombre { get; set; }
        public bool FlagActivo { get; set; }
        public List<MenuBE> ListaMenu { get; set; }
    }
}