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
    public class MenuSistemaBL : BaseBL
    {
        MenuSistemaDA menuDA = new MenuSistemaDA();

        public bool GuardarMenuSistema(MenuSistemaBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            mensajeError = null;
            bool seGuardo = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seGuardo = menuDA.GuardarMenuSistema(cn, registro, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public List<MenuSistemaBE> BuscarMenuSistema(string nombre, string url, string nombrePadre, int? menuIdPadre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, out string mensajeError)
        {
            List<MenuSistemaBE> lista = null;
            totalRows = 0;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = menuDA.BuscarMenuSistema(cn, nombre, url, nombrePadre, menuIdPadre, pageNumber, pageSize, sortName, sortOrder, out totalRows);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public bool CambiarFlagActivoMenuSistema(int menuId, string usuarioIdModificacion, bool flagActivo, out string mensajeError)
        {
            mensajeError = null;
            bool seCambio = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seCambio = menuDA.CambiarFlagActivoMenuSistema(cn, menuId, flagActivo, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seCambio;
        }

        public bool CambiarOrdenMenuSistema(int menuId, string usuarioIdModificacion, int orden, out string mensajeError)
        {
            mensajeError = null;
            bool seCambio = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seCambio = menuDA.CambiarOrdenMenuSistema(cn, menuId, orden, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seCambio;
        }

        public MenuSistemaBE ObtenerMenuSistema(int menuId, out string mensajeError)
        {
            MenuSistemaBE item = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = menuDA.ObtenerMenuSistema(cn, menuId);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return item;
        }

        public bool ExisteNombreMenuSistema(int? menuId, string nombre, int? menuIdPadre, out string mensajeError)
        {
            bool existe = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    existe = menuDA.ExisteNombreMenuSistema(cn, menuId, nombre, menuIdPadre);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return existe;
        }

        public List<MenuSistemaBE> ListarMenuSistemaPorFlagActivo(bool? flagActivo, out string mensajeError)
        {
            List<MenuSistemaBE> lista = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = menuDA.ListarMenuSistemaPorFlagActivo(cn, flagActivo);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public List<MenuSistemaBE> ListarMenuSistemaPorFlagActivoMenuIdPadre(bool? flagActivo, int? menuIdPadre, out string mensajeError, bool conListaSubMenu = false)
        {
            List<MenuSistemaBE> lista = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = menuDA.ListarMenuSistemaPorFlagActivoMenuIdPadre(cn, flagActivo, menuIdPadre);
                    if (conListaSubMenu)
                    {
                        if (lista != null)
                        {
                            if (conListaSubMenu) lista = ListarMenuSistemaPorFlagActivoMenuIdPadreConSubMenu(cn, null, lista, flagActivo);
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

        private List<MenuSistemaBE> ListarMenuSistemaPorFlagActivoMenuIdPadreConSubMenu(SqlConnection cn, MenuSistemaBE item, List<MenuSistemaBE> lista, bool? flagActivo)
        {
            if (item != null)
            {
                item.ListaSubMenu = menuDA.ListarMenuSistemaPorFlagActivoMenuIdPadre(cn, flagActivo, item.MenuId);
                lista = item.ListaSubMenu;
            }

            if (lista != null)
            {
                foreach (MenuSistemaBE x in lista)
                {
                    x.ListaSubMenu = ListarMenuSistemaPorFlagActivoMenuIdPadreConSubMenu(cn, x, x.ListaSubMenu, flagActivo);
                }
            }

            return lista;
        }

        public List<MenuSistemaBE> ListarMenuSistemaPorFlagActivoPerfil(int perfilId, bool? flagActivo, out string mensajeError)
        {
            List<MenuSistemaBE> lista = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = menuDA.ListarMenuSistemaPorFlagActivoPerfil(cn, perfilId, flagActivo);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }
    }
}
