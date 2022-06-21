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
    public class TokenUsuarioSistemaDA
    {
        public bool GuardarTokenUsuarioSistema(SqlConnection cn, TokenUsuarioSistemaBE registro, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_tokenusuariosistema_guardar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if(tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@tokenId", registro.TokenId);
                cmd.Parameters.AddWithValue("@usuarioId", registro.UsuarioId);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seGuardo = filasAfectadas > 0;
            }

            return seGuardo;
        }
    }
}
