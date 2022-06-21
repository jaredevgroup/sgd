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
    public class UsuarioSistemaDA
    {
        public UsuarioSistemaBE ObtenerUsuarioSistema(SqlConnection cn, string usuarioId)
        {
            UsuarioSistemaBE item = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuariosistema_obtener", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuarioSistemaId", usuarioId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            item = new UsuarioSistemaBE();
                            item.UsuarioId = dr.GetData<string>("UsuarioSistemaId");
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

        public UsuarioSistemaBE ObtenerUsuarioSistemaPorCorreo(SqlConnection cn, string correo)
        {
            UsuarioSistemaBE item = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuariosistema_obtener_x_correo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo", correo);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            item = new UsuarioSistemaBE();
                            item.UsuarioId = dr.GetData<string>("UsuarioSistemaId");
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

        public bool GuardarContraseñaUsuarioSistema(SqlConnection cn, UsuarioSistemaBE registro, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuariosistema_guardar_contraseña", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@usuarioSistemaId", registro.UsuarioId);
                cmd.Parameters.AddWithValue("@contraseña", registro.ObtenerContraseñaCodificada());
                int filasAfectadas = cmd.ExecuteNonQuery();
                seGuardo = filasAfectadas > 0;
            }

            return seGuardo;
        }

        public List<UsuarioSistemaBE> BuscarUsuarioSistema(SqlConnection cn, string usuarioId, string correo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            List<UsuarioSistemaBE> lista = null;
            totalRows = 0;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuariosistema_buscar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuarioSistemaId", usuarioId.GetNullable());
                cmd.Parameters.AddWithValue("@correo", correo.GetNullable());
                cmd.Parameters.AddWithValue("@pageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@sortName", sortName);
                cmd.Parameters.AddWithValue("@sortOrder", sortOrder);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<UsuarioSistemaBE>();
                        while (dr.Read())
                        {
                            UsuarioSistemaBE item = new UsuarioSistemaBE();
                            item.UsuarioId = dr.GetData<string>("UsuarioSistemaId");
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

        public bool CambiarFlagActivoUsuarioSistema(SqlConnection cn, string usuarioId, bool flagActivo, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seCambio = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuariosistema_cambiar_flagactivo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@usuarioSistemaId", usuarioId);
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo);
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seCambio = filasAfectadas > 0;
            }

            return seCambio;
        }

        public bool ExisteIdUsuarioSistema(SqlConnection cn, int tipoMantenimiento, string usuarioId)
        {
            bool existe = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuariosistema_usuarioid_existe", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipoMantenimiento", tipoMantenimiento);
                cmd.Parameters.AddWithValue("@usuarioSistemaId", usuarioId.GetNullable());

                existe = (bool)cmd.ExecuteScalar();
            }

            return existe;
        }

        public bool ExisteCorreoUsuarioSistema(SqlConnection cn, string usuarioId, string correo)
        {
            bool existe = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuariosistema_correo_existe", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuarioSistemaId", usuarioId.GetNullable());
                cmd.Parameters.AddWithValue("@correo", correo.GetNullable());

                existe = (bool)cmd.ExecuteScalar();
            }

            return existe;
        }

        public bool GuardarUsuarioSistema(SqlConnection cn, int tipoMantenimiento, UsuarioSistemaBE registro, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_usuariosistema_guardar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@tipoMantenimiento", tipoMantenimiento);
                cmd.Parameters.AddWithValue("@usuarioSistemaId", registro.UsuarioId);
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
