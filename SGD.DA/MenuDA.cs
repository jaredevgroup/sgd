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
    public class MenuDA
    {
        public bool GuardarMenu(SqlConnection cn, MenuBE registro, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seGuardo = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_guardar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@menuId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.InputOutput, Value = registro.MenuId });
                cmd.Parameters.AddWithValue("@nombre", registro.Nombre);
                cmd.Parameters.AddWithValue("@url", registro.Url);
                cmd.Parameters.AddWithValue("@icono", registro.Icono);
                cmd.Parameters.AddWithValue("@menuIdPadre", registro.MenuIdPadre.GetNullable());
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seGuardo = filasAfectadas > 0;
            }

            return seGuardo;
        }

        public List<MenuBE> BuscarMenu(SqlConnection cn, string nombre, string url, string nombrePadre, int? menuIdPadre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            List<MenuBE> lista = null;
            totalRows = 0;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_buscar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", nombre.GetNullable());
                cmd.Parameters.AddWithValue("@url", url.GetNullable());
                cmd.Parameters.AddWithValue("@nombrePadre", nombrePadre.GetNullable());
                cmd.Parameters.AddWithValue("@menuIdPadre", menuIdPadre.GetNullable());
                cmd.Parameters.AddWithValue("@pageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@sortName", sortName);
                cmd.Parameters.AddWithValue("@sortOrder", sortOrder);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<MenuBE>();
                        while (dr.Read())
                        {
                            MenuBE item = new MenuBE();
                            item.MenuId = dr.GetData<int>("MenuId");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.Url = dr.GetData<string>("Url");
                            item.Icono = dr.GetData<string>("Icono");
                            item.Orden = dr.GetData<int>("Orden");
                            item.MenuIdPadre = dr.GetData<int?>("MenuIdPadre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");
                            item.FlagSubMenu = dr.GetData<bool>("FlagSubMenu");

                            totalRows = dr.GetData<int>("TotalRows");

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public bool CambiarFlagActivoMenu(SqlConnection cn, int menuId, bool flagActivo, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seCambio = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_cambiar_flagactivo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@menuId", menuId);
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo);
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seCambio = filasAfectadas > 0;
            }

            return seCambio;
        }

        public bool CambiarOrdenMenu(SqlConnection cn, int menuId, int orden, string usuarioIdModificacion, SqlTransaction tran = null)
        {
            bool seCambio = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_cambiar_orden", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@menuId", menuId);
                cmd.Parameters.AddWithValue("@orden", orden);
                cmd.Parameters.AddWithValue("@usuarioIdModificacion", usuarioIdModificacion);
                int filasAfectadas = cmd.ExecuteNonQuery();
                seCambio = filasAfectadas > 0;
            }

            return seCambio;
        }

        public MenuBE ObtenerMenu(SqlConnection cn, int menuId)
        {
            MenuBE item = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_obtener", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@menuId", menuId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        item = new MenuBE();
                        if (dr.Read())
                        {
                            item.MenuId = dr.GetData<int>("MenuId");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.Url = dr.GetData<string>("Url");
                            item.Icono = dr.GetData<string>("Icono");
                            item.Orden = dr.GetData<int>("Orden");
                            item.MenuIdPadre = dr.GetData<int?>("MenuIdPadre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");
                        }
                    }
                }
            }

            return item;
        }

        public bool ExisteNombreMenu(SqlConnection cn, int? menuId, string nombre, int? menuIdPadre)
        {
            bool existe = false;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_nombre_existe", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@menuId", menuId.GetNullable());
                cmd.Parameters.AddWithValue("@nombre", nombre.GetNullable());
                cmd.Parameters.AddWithValue("@menuIdPadre", menuIdPadre.GetNullable());

                existe = (bool)cmd.ExecuteScalar();
            }

            return existe;
        }

        public List<MenuBE> ListarMenuPorFlagActivo(SqlConnection cn, bool? flagActivo)
        {
            List<MenuBE> lista = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_listar_x_flagactivo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo.GetNullable());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<MenuBE>();
                        while (dr.Read())
                        {
                            MenuBE item = new MenuBE();
                            item.MenuId = dr.GetData<int>("MenuId");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.Url = dr.GetData<string>("Url");
                            item.Icono = dr.GetData<string>("Icono");
                            item.Orden = dr.GetData<int>("Orden");
                            item.MenuIdPadre = dr.GetData<int?>("MenuIdPadre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public List<MenuBE> ListarMenuPorFlagActivoMenuIdPadre(SqlConnection cn, bool? flagActivo, int? menuIdPadre)
        {
            List<MenuBE> lista = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_listar_x_flagactivo_menuidpadre", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo.GetNullable());
                cmd.Parameters.AddWithValue("@menuIdPadre", menuIdPadre.GetNullable());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<MenuBE>();
                        while (dr.Read())
                        {
                            MenuBE item = new MenuBE();
                            item.MenuId = dr.GetData<int>("MenuId");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.Url = dr.GetData<string>("Url");
                            item.Icono = dr.GetData<string>("Icono");
                            item.Orden = dr.GetData<int>("Orden");
                            item.MenuIdPadre = dr.GetData<int?>("MenuIdPadre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        //public List<MenuBE> ListarMenuPorFlagActivoPerfil(SqlConnection cn, int perfilId, bool? flagActivo)
        //{
        //    List<MenuBE> lista = null;

        //    using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_listar_x_flagactivo_perfil", cn))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@flagActivo", flagActivo.GetNullable());
        //        cmd.Parameters.AddWithValue("@perfilId", perfilId);

        //        using (SqlDataReader dr = cmd.ExecuteReader())
        //        {
        //            if (dr.HasRows)
        //            {
        //                lista = new List<MenuBE>();
        //                while (dr.Read())
        //                {
        //                    MenuBE item = new MenuBE();
        //                    item.MenuId = dr.GetData<int>("MenuId");
        //                    item.Nombre = dr.GetData<string>("Nombre");
        //                    item.Url = dr.GetData<string>("Url");
        //                    item.Icono = dr.GetData<string>("Icono");
        //                    item.Orden = dr.GetData<int>("Orden");
        //                    item.MenuIdPadre = dr.GetData<int?>("MenuIdPadre");
        //                    item.FlagActivo = dr.GetData<bool>("FlagActivo");

        //                    lista.Add(item);
        //                }
        //            }
        //        }
        //    }

        //    return lista;
        //}

        //public List<MenuBE> ListarMenuPorFlagActivoPerfiles(SqlConnection cn, string perfilesId, bool? flagActivo)
        //{
        //    List<MenuBE> lista = null;

        //    using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_listar_x_flagactivo_perfiles", cn))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@flagActivo", flagActivo.GetNullable());
        //        cmd.Parameters.AddWithValue("@perfilesId", perfilesId);

        //        using (SqlDataReader dr = cmd.ExecuteReader())
        //        {
        //            if (dr.HasRows)
        //            {
        //                lista = new List<MenuBE>();
        //                while (dr.Read())
        //                {
        //                    MenuBE item = new MenuBE();
        //                    item.MenuId = dr.GetData<int>("MenuId");
        //                    item.Nombre = dr.GetData<string>("Nombre");
        //                    item.Url = dr.GetData<string>("Url");
        //                    item.Icono = dr.GetData<string>("Icono");
        //                    item.Orden = dr.GetData<int>("Orden");
        //                    item.MenuIdPadre = dr.GetData<int?>("MenuIdPadre");
        //                    item.FlagActivo = dr.GetData<bool>("FlagActivo");

        //                    lista.Add(item);
        //                }
        //            }
        //        }
        //    }

        //    return lista;
        //}

        public List<MenuBE> ListarMenuPorFlagActivoEntidadDeportiva(SqlConnection cn, int entidadDeportivaId, bool? flagActivo)
        {
            List<MenuBE> lista = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_listar_x_flagactivo_entidaddeportiva", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId);
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo.GetNullable());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<MenuBE>();
                        while (dr.Read())
                        {
                            MenuBE item = new MenuBE();
                            item.MenuId = dr.GetData<int>("MenuId");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.Url = dr.GetData<string>("Url");
                            item.Icono = dr.GetData<string>("Icono");
                            item.Orden = dr.GetData<int>("Orden");
                            item.MenuIdPadre = dr.GetData<int?>("MenuIdPadre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public List<MenuBE> ListarMenuPorFlagActivoMenuIdPadreEntidadDeportiva(SqlConnection cn, int entidadDeportivaId, bool? flagActivo, int? menuIdPadre)
        {
            List<MenuBE> lista = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_listar_x_flagactivo_menuidpadre_entidaddeportiva", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId);
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo.GetNullable());
                cmd.Parameters.AddWithValue("@menuIdPadre", menuIdPadre.GetNullable());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<MenuBE>();
                        while (dr.Read())
                        {
                            MenuBE item = new MenuBE();
                            item.MenuId = dr.GetData<int>("MenuId");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.Url = dr.GetData<string>("Url");
                            item.Icono = dr.GetData<string>("Icono");
                            item.Orden = dr.GetData<int>("Orden");
                            item.MenuIdPadre = dr.GetData<int?>("MenuIdPadre");
                            item.FlagActivo = dr.GetData<bool>("FlagActivo");

                            lista.Add(item);
                        }
                    }
                }
            }

            return lista;
        }

        public List<MenuBE> ListarMenuPorFlagActivoEntidadDeportivaPerfil(SqlConnection cn, int entidadDeportivaId, int perfilId, bool? flagActivo)
        {
            List<MenuBE> lista = null;

            using (SqlCommand cmd = new SqlCommand("dbo.usp_menu_listar_x_flagactivo_entidaddeportivaperfil", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@entidadDeportivaId", entidadDeportivaId);
                cmd.Parameters.AddWithValue("@flagActivo", flagActivo.GetNullable());
                cmd.Parameters.AddWithValue("@perfilId", perfilId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        lista = new List<MenuBE>();
                        while (dr.Read())
                        {
                            MenuBE item = new MenuBE();
                            item.MenuId = dr.GetData<int>("MenuId");
                            item.Nombre = dr.GetData<string>("Nombre");
                            item.Url = dr.GetData<string>("Url");
                            item.Icono = dr.GetData<string>("Icono");
                            item.Orden = dr.GetData<int>("Orden");
                            item.MenuIdPadre = dr.GetData<int?>("MenuIdPadre");
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
