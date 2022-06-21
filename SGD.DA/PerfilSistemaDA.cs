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
    public class PerfilSistemaDA
    {
        public bool GuardarPerfilSistema(SqlConnection cn, PerfilSistemaBE registro, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_perfilsistema_guardar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@perfilSistemaId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.InputOutput, Value = registro.PerfilId });
                cmd.Parameters.AddWithValue("@nombre", registro.Nombre);
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seGuardo = filasAfectadas > 0;
            }

            return seGuardo;
        }

        public List<PerfilSistemaBE> BuscarPerfilSistema(SqlConnection cn, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            List<PerfilSistemaBE> lista = null;
            totalRows = 0;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_perfilsistema_buscar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", nombre.GetNullable());
                cmd.Parameters.AddWithValue("@pageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@sortName", sortName);
                cmd.Parameters.AddWithValue("@sortOrder", sortOrder);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<PerfilSistemaBE>();
                        while (dr.Read())
                        {
                            PerfilSistemaBE item = new PerfilSistemaBE();
                            item.PerfilId = dr.GetData<int>("PerfilSistemaId");
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

        public bool CambiarFlagActivoPerfilSistema(SqlConnection cn, int perfilId, bool flagActivo, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seCambio = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_perfilsistema_cambiar_flagactivo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@perfilSistemaId", perfilId);
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo);
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seCambio = filasAfectadas > 0;
            }

            return seCambio;
        }

        public PerfilSistemaBE ObtenerPerfilSistema(SqlConnection cn, int perfilId)
        {
            PerfilSistemaBE item = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_perfilsistema_obtener", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@perfilSistemaId", perfilId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        item = new PerfilSistemaBE();
                        if (dr.Read())
                        {
                            item.PerfilId = dr.GetData<int>("PerfilSistemaId");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");
                        }
                    }
                }
            }

            return item;
        }

        public bool ExisteNombrePerfilSistema(SqlConnection cn, int? perfilId, string nombre)
        {
            bool existe = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_perfilsistema_nombre_existe", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@perfilSistemaId", perfilId.GetNullable());
                cmd.Parameters.AddWithValue("@nombre", nombre.GetNullable());

                existe = (bool)cmd.ExecuteScalar();
            }

            return existe;
        }

        public List<PerfilSistemaBE> ListarPerfilSistemaPorFlagActivo(SqlConnection cn, bool? flagActivo)
        {
            List<PerfilSistemaBE> lista = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_perfilsistema_listar_x_flagactivo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo.GetNullable());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<PerfilSistemaBE>();
                        while (dr.Read())
                        {
                            PerfilSistemaBE item = new PerfilSistemaBE();
                            item.PerfilId = dr.GetData<int>("PerfilSistemaId");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public List<PerfilSistemaBE> ListarPerfilSistemaPorFlagActivoUsuario(SqlConnection cn, string usuarioId, bool? flagActivo)
        {
            List<PerfilSistemaBE> lista = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_perfilsistema_listar_x_flagactivo_usuario", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo.GetNullable());
                cmd.Parameters.AddWithValue("@usuarioSistemaId", usuarioId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<PerfilSistemaBE>();
                        while (dr.Read())
                        {
                            PerfilSistemaBE item = new PerfilSistemaBE();
                            item.PerfilId = dr.GetData<int>("PerfilSistemaId");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }
    }
}