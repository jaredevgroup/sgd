using SGD.BE;
using SGD.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.DA
{
    public class PlantillaDA
    {
        public PlantillaBE ObtenerPlantillaPorTipoPlantilla(SqlConnection cn, int enumTipoPlantilla)
        {
            PlantillaBE item = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_plantilla_obtener_x_enumtipoplantilla", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@enumTipoPlantilla", enumTipoPlantilla);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            item = new PlantillaBE();
                            item.PlantillaId = dr.GetData<int>("PlantillaId");
                            int enumTipoPlantillaValor = dr.GetData<int>("EnumTipoPlantilla");
                            item.EnumTipoPlantilla = (Enumeracion.TipoPlantilla)enumTipoPlantillaValor;
                            item.Texto = dr.GetData<string>("Texto");
                        }
                    }
                }
            }

            return item;
        }
    }
}
