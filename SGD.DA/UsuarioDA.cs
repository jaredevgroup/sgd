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
    public class UsuarioDA
    {
        public UsuarioBE ObtenerUsuario(SqlConnection cn, int entidadDeportivaId, string usuarioId)
        {
            UsuarioBE item = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuario_obtener", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId);
                cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            item = new UsuarioBE();
                            item.EntidadDeportivaId = dr.GetData<int>("EntidadDeportivaId");
                            item.UsuarioId = dr.GetData<string>("UsuarioId");
                            item.ContraseñaByte = dr.GetData<byte[]>("Contraseña");
                            item.Correo = dr.GetData<string>("Correo");
                            item.FlagCambioContraseña = dr.GetData<bool>("FlagCambioContraseña");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");
                        }
                    }
                }
            }

            return item;
        }

        public UsuarioBE ObtenerUsuarioPorCorreo(SqlConnection cn, int entidadDeportivaId, string correo)
        {
            UsuarioBE item = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuario_obtener_x_correo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId);
                cmd.Parameters.AddWithValue("@correo", correo);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            item = new UsuarioBE();
                            item.EntidadDeportivaId = dr.GetData<int>("EntidadDeportivaId");
                            item.UsuarioId = dr.GetData<string>("UsuarioId");
                            item.ContraseñaByte = dr.GetData<byte[]>("Contraseña");
                            item.Correo = dr.GetData<string>("Correo");
                            item.FlagCambioContraseña = dr.GetData<bool>("FlagCambioContraseña");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");
                        }
                    }
                }
            }

            return item;
        }

        public bool GuardarContraseñaUsuario(SqlConnection cn, UsuarioBE registro, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuario_guardar_contraseña", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", registro.EntidadDeportivaId);
                cmd.Parameters.AddWithValue("@usuarioId", registro.UsuarioId);
                cmd.Parameters.AddWithValue("@contraseña", registro.ObtenerContraseñaCodificada());
                int filasAfectadas = cmd.ExecuteNonQuery();
                seGuardo = filasAfectadas > 0;
            }

            return seGuardo;
        }

        public List<UsuarioBE> BuscarUsuario(SqlConnection cn, int entidadDeportivaId, string usuarioId, string correo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            List<UsuarioBE> lista = null;
            totalRows = 0;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuario_buscar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId.GetNullable());
                cmd.Parameters.AddWithValue("@usuarioId", usuarioId.GetNullable());
                cmd.Parameters.AddWithValue("@correo", correo.GetNullable());
                cmd.Parameters.AddWithValue("@pageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@sortName", sortName);
                cmd.Parameters.AddWithValue("@sortOrder", sortOrder);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<UsuarioBE>();
                        while (dr.Read())
                        {
                            UsuarioBE item = new UsuarioBE();
                            item.EntidadDeportivaId = dr.GetData<int>("EntidadDeportivaId");
                            item.UsuarioId = dr.GetData<string>("UsuarioId");
                            item.Correo = dr.GetData<string>("Correo");
                            item.FlagCambioContraseña = dr.GetData<bool>("FlagCambioContraseña");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");

                            totalRows = dr.GetData<int>("TotalRows");

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public bool CambiarFlagActivoUsuario(SqlConnection cn, int entidadDeportivaId, string usuarioId, bool flagActivo, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seCambio = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuario_cambiar_flagactivo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId);
                cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo);
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seCambio = filasAfectadas > 0;
            }

            return seCambio;
        }

        public bool ExisteIdUsuario(SqlConnection cn, int tipoMantenimiento, int entidadDeportivaId, string usuarioId)
        {
            bool existe = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuario_usuarioid_existe", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipoMantenimiento", tipoMantenimiento);
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId);
                cmd.Parameters.AddWithValue("@usuarioId", usuarioId.GetNullable());

                existe = (bool)cmd.ExecuteScalar();
            }

            return existe;
        }

        public bool ExisteCorreoUsuario(SqlConnection cn, int entidadDeportivaId, string usuarioId, string correo)
        {
            bool existe = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuario_correo_existe", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId);
                cmd.Parameters.AddWithValue("@usuarioId", usuarioId.GetNullable());
                cmd.Parameters.AddWithValue("@correo", correo.GetNullable());

                existe = (bool)cmd.ExecuteScalar();
            }

            return existe;
        }

        public bool GuardarUsuario(SqlConnection cn, int tipoMantenimiento, UsuarioBE registro, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuario_guardar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@tipoMantenimiento", tipoMantenimiento);
                cmd.Parameters.AddWithValue("@entidadDeportivaId", registro.EntidadDeportivaId);
                cmd.Parameters.AddWithValue("@usuarioId", registro.UsuarioId);
                cmd.Parameters.AddWithValue("@contraseña", registro.ObtenerContraseñaCodificada());
                cmd.Parameters.AddWithValue("@correo", registro.Correo);
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seGuardo = filasAfectadas > 0;
            }

            return seGuardo;
        }
    }
}
