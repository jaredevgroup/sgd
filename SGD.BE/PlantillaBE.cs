using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SGD.Util.Enumeracion;

namespace SGD.BE
{
    public class PlantillaBE
    {
        public int PlantillaId { get; set; }
        public TipoPlantilla EnumTipoPlantilla { get; set; }
        public string Texto { get; set; }

        #region Métodos
        public int ObtenerValorEnumTipoPlantilla()
        {
            int valorTipoPlantilla = (int)EnumTipoPlantilla;
            return valorTipoPlantilla;
        }
        #endregion
    }
}
