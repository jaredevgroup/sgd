using SGD.BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.DA
{
    public class TokenUsuarioDA
    {
        public bool GuardarTokenUsuario(SqlConnection cn, TokenUsuarioBE registro, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_tokenusuario_guardar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@tokenId", registro.TokenId);
                cmd.Parameters.AddWithValue("@entidadDeportivaId", registro.EntidadDeportivaId);
                cmd.Parameters.AddWithValue("@usuarioId", registro.UsuarioId);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seGuardo = filasAfectadas > 0;
            }

            return seGuardo;
        }
    }
}
