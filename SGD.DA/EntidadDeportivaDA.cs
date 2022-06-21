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
    public class EntidadDeportivaDA
    {
        public List<EntidadDeportivaBE> BuscarEntidadDeportiva(SqlConnection cn, string codigo, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            List<EntidadDeportivaBE> lista = null;
            totalRows = 0;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_entidaddeportiva_buscar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", codigo.GetNullable());
                cmd.Parameters.AddWithValue("@nombre", nombre.GetNullable());
                cmd.Parameters.AddWithValue("@pageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@sortName", sortName);
                cmd.Parameters.AddWithValue("@sortOrder", sortOrder);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<EntidadDeportivaBE>();
                        while (dr.Read())
                        {
                            EntidadDeportivaBE item = new EntidadDeportivaBE();
                            item.EntidadDeportivaId = dr.GetData<int>("EntidadDeportivaId");
                            item.Codigo = dr.GetData<string>("Codigo");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");

                            totalRows = dr.GetData<int>("TotalRows");

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public bool GuardarEntidadDeportiva(SqlConnection cn, EntidadDeportivaBE registro, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_entidaddeportiva_guardar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@entidadDeportivaId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.InputOutput, Value = registro.EntidadDeportivaId });
                cmd.Parameters.AddWithValue("@codigo", registro.Codigo);
                cmd.Parameters.AddWithValue("@nombre", registro.Nombre);
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seGuardo = filasAfectadas > 0;
            }

            return seGuardo;
        }

        public bool CambiarFlagActivoEntidadDeportiva(SqlConnection cn, int entidadDeportivaId, bool flagActivo, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seCambio = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_entidaddeportiva_cambiar_flagactivo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId);
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo);
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seCambio = filasAfectadas > 0;
            }

            return seCambio;
        }

        public EntidadDeportivaBE ObtenerEntidadDeportiva(SqlConnection cn, int entidadDeportivaId)
        {
            EntidadDeportivaBE item = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_entidaddeportiva_obtener", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        item = new EntidadDeportivaBE();
                        if (dr.Read())
                        {
                            item.EntidadDeportivaId = dr.GetData<int>("EntidadDeportivaId");
                            item.Codigo = dr.GetData<string>("Codigo");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");
                        }
                    }
                }
            }

            return item;
        }

        public bool ExisteCodigoEntidadDeportiva(SqlConnection cn, int? entidadDeportivaId, string codigo)
        {
            bool existe = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_entidaddeportiva_codigo_existe", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId.GetNullable());
                cmd.Parameters.AddWithValue("@codigo", codigo.GetNullable());

                existe = (bool)cmd.ExecuteScalar();
            }

            return existe;
        }
    }
}
