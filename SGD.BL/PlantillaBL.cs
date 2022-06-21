using SGD.BE;
using SGD.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BL
{
    public class PlantillaBL : BaseBL
    {
        PlantillaDA plantillaDA = new PlantillaDA();

        public PlantillaBE ObtenerPlantillaPorTipoPlantilla(int enumTipoPlantilla)
        {
            PlantillaBE item = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = plantillaDA.ObtenerPlantillaPorTipoPlantilla(cn, enumTipoPlantilla);
                    cn.Close();
                }
                catch (SqlException ex) { }
                catch (Exception ex) { }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return item;
        }
    }
}
