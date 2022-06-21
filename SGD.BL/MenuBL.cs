using SGD.BE;
using SGD.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BL
{
    public class MenuBL : BaseBL
    {
        MenuDA menuDA = new MenuDA();

        public bool GuardarMenu(MenuBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            mensajeError = null;
            bool seGuardo = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seGuardo = menuDA.GuardarMenu(cn, registro, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public List<MenuBE> BuscarMenu(string nombre, string url, string nombrePadre, int? menuIdPadre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, out string mensajeError)
        {
            List<MenuBE> lista = null;
            totalRows = 0;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = menuDA.BuscarMenu(cn, nombre, url, nombrePadre, menuIdPadre, pageNumber, pageSize, sortName, sortOrder, out totalRows);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public bool CambiarFlagActivoMenu(int menuId, string usuarioIdModificacion, bool flagActivo, out string mensajeError)
        {
            mensajeError = null;
            bool seCambio = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seCambio = menuDA.CambiarFlagActivoMenu(cn, menuId, flagActivo, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seCambio;
        }

        public bool CambiarOrdenMenu(int menuId, string usuarioIdModificacion, int orden, out string mensajeError)
        {
            mensajeError = null;
            bool seCambio = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seCambio = menuDA.CambiarOrdenMenu(cn, menuId, orden, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seCambio;
        }

        public MenuBE ObtenerMenu(int menuId, out string mensajeError)
        {
            MenuBE item = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = menuDA.ObtenerMenu(cn, menuId);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return item;
        }

        public bool ExisteNombreMenu(int? menuId, string nombre, int? menuIdPadre, out string mensajeError)
        {
            bool existe = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    existe = menuDA.ExisteNombreMenu(cn, menuId, nombre, menuIdPadre);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return existe;
        }

        public List<MenuBE> ListarMenuPorFlagActivo(bool? flagActivo, out string mensajeError)
        {
            List<MenuBE> lista = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = menuDA.ListarMenuPorFlagActivo(cn, flagActivo);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public List<MenuBE> ListarMenuPorFlagActivoMenuIdPadre(bool? flagActivo, int? menuIdPadre, out string mensajeError, bool conListaSubMenu = false)
        {
            List<MenuBE> lista = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = menuDA.ListarMenuPorFlagActivoMenuIdPadre(cn, flagActivo, menuIdPadre);
                    if (conListaSubMenu)
                    {
                        if (lista != null)
                        {
                            if (conListaSubMenu) lista = ListarMenuPorFlagActivoMenuIdPadreConSubMenu(cn, null, lista, flagActivo);
                        }
                    }
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        private List<MenuBE> ListarMenuPorFlagActivoMenuIdPadreConSubMenu(SqlConnection cn, MenuBE item, List<MenuBE> lista, bool? flagActivo)
        {
            if (item != null)
            {
                item.ListaSubMenu = menuDA.ListarMenuPorFlagActivoMenuIdPadre(cn, flagActivo, item.MenuId);
                lista = item.ListaSubMenu;
            }

            if (lista != null)
            {
                foreach (MenuBE x in lista)
                {
                    x.ListaSubMenu = ListarMenuPorFlagActivoMenuIdPadreConSubMenu(cn, x, x.ListaSubMenu, flagActivo);
                }
            }

            return lista;
        }

        //public List<MenuBE> ListarMenuPorFlagActivoPerfil(int perfilId, bool? flagActivo, out string mensajeError)
        //{
        //    List<MenuBE> lista = null;
        //    mensajeError = null;

        //    using (SqlConnection cn = new SqlConnection(cnString))
        //    {
        //        try
        //        {
        //            cn.Open();
        //            lista = menuDA.ListarMenuPorFlagActivoPerfil(cn, perfilId, flagActivo);
        //            cn.Close();
        //        }
        //        catch (SqlException ex) { mensajeError = ex.Message; }
        //        catch (Exception ex) { mensajeError = ex.Message; }
        //        finally { if (cn.State == ConnectionState.Open) cn.Close(); }
        //    }

        //    return lista;
        //}
    }
}
