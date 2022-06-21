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
    public class TokenDA
    {
        public bool GuardarToken(SqlConnection cn, TokenBE registro, out string tokenId, out DateTime? fechaCreacion, SqlTransaction tran = null)
        {
            bool seGuardo = false;
            tokenId = null;
            fechaCreacion = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_token_guardar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if(tran != null) cmd.Transaction = tran;
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@tokenId", SqlDbType = SqlDbType.VarChar, Size = 36, Direction = ParameterDirection.Output });
                cmd.Parameters.AddWithValue("@enumTipoToken", registro.ObtenerValorEnumTipoToken());
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@fechaCreacion", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Output });
                cmd.Parameters.AddWithValue("@milisegundosDuracion", registro.MilisegundosDuracion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seGuardo = filasAfectadas > 0;
                if (seGuardo)
                {
                    tokenId = (string)cmd.Parameters["@tokenId"].Value;
                    fechaCreacion = (DateTime?)cmd.Parameters["@fechaCreacion"].Value;
                }
            }

            return seGuardo;
        }

        public TokenBE ObtenerTokenPorUsuarioSistema(SqlConnection cn, string tokenId, string usuarioId)
        {
            TokenBE item = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_token_obtener_x_usuariosistema", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tokenId", tokenId);
                cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            item = new TokenBE();
                            item.TokenId = dr.GetData<string>("TokenId");
                            int enumTipoTokenValor = dr.GetData<int>("EnumTipoToken");
                            item.EnumTipoToken = (Enumeracion.TipoToken)enumTipoTokenValor;
                            item.FechaCreacion = dr.GetData<DateTime>("FechaCreacion");
                            item.MilisegundosDuracion = dr.GetData<long>("MilisegundosDuracion");
                        }
                    }
                }
            }

            return item;
        }
    }
}
