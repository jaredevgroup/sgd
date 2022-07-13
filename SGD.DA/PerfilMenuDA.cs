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
    public class PerfilMenuDA
    {
        public bool GuardarPerfilMenu(SqlConnection cn, PerfilMenuBE registro, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_perfilmenu_guardar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
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
