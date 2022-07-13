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
    public  class EntidadDeportivaPerfilMenuDA
    {
        public bool GuardarEntidadDeportivaPerfilMenu(SqlConnection cn, EntidadDeportivaPerfilMenuBE registro, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_entidaddeportivaperfilmenu_guardar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", registro.EntidadDeportivaId);
                cmd.Parameters.AddWithValue("@perfilId", registro.PerfilId);
                cmd.Parameters.AddWithValue("@menuId", registro.MenuId);
                cmd.Parameters.AddWithValue("@flagActivo", registro.FlagActivo);
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seGuardo = filasAfectadas != -1;
            }

            return seGuardo;
        }
    }
}
