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
    public class EntidadDeportivaBL : BaseBL
    {
        EntidadDeportivaDA entidadDeportivaDA = new EntidadDeportivaDA();
        EntidadDeportivaMenuDA entidadDeportivaMenuDA = new EntidadDeportivaMenuDA();
        EntidadDeportivaPerfilDA entidadDeportivaPerfilDA = new EntidadDeportivaPerfilDA();
        EntidadDeportivaPerfilMenuDA entidadDeportivaPerfilMenuDA = new EntidadDeportivaPerfilMenuDA();
        MenuDA menuDA = new MenuDA();
        PerfilDA perfilDA = new PerfilDA();

        public List<EntidadDeportivaBE> BuscarEntidadDeportiva(string codigo, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, out string mensajeError)
        {
            List<EntidadDeportivaBE> lista = null;
            totalRows = 0;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = entidadDeportivaDA.BuscarEntidadDeportiva(cn, codigo, nombre, pageNumber, pageSize, sortName, sortOrder, out totalRows);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public bool GuardarEntidadDeportiva(EntidadDeportivaBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            bool seGuardo = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seGuardo = entidadDeportivaDA.GuardarEntidadDeportiva(cn, registro, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public bool CambiarFlagActivoEntidadDeportiva(int entidadDeportivaId, bool flagActivo, string usuarioIdModificacion, out string mensajeError)
        {
            bool seCambio = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seCambio = entidadDeportivaDA.CambiarFlagActivoEntidadDeportiva(cn, entidadDeportivaId, flagActivo, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seCambio;
        }

        public EntidadDeportivaBE ObtenerEntidadDeportiva(int entidadDeportivaId, out string mensajeError, bool conListaPerfil = false, bool conListaMenu = false, bool conListaSubMenu = false)
        {
            EntidadDeportivaBE item = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = entidadDeportivaDA.ObtenerEntidadDeportiva(cn, entidadDeportivaId);
                    if(item != null)
                    {
                        if(conListaMenu || conListaPerfil)
                        {
                            if (conListaMenu) {
                                if (conListaSubMenu)
                                {
                                    item.ListaMenu = menuDA.ListarMenuPorFlagActivoMenuIdPadreEntidadDeportiva(cn, entidadDeportivaId, conListaMenu, null);
                                    item.ListaMenu = ListarMenuPorFlagActivoMenuIdPadreEntidadDeportivaConSubMenu(cn, item.EntidadDeportivaId, null, item.ListaMenu, conListaMenu);
                                }
                                else item.ListaMenu = menuDA.ListarMenuPorFlagActivoEntidadDeportiva(cn, entidadDeportivaId, conListaMenu);
                            }
                            if (conListaPerfil) item.ListaPerfil = perfilDA.ListarPerfilPorFlagActivoEntidadDeportiva(cn, entidadDeportivaId, conListaPerfil);
                        }
                    }
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return item;
        }

        public bool ExisteCodigoEntidadDeportiva(int? entidadDeportivaId, string codigo, out string mensajeError)
        {
            bool existe = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    existe = entidadDeportivaDA.ExisteCodigoEntidadDeportiva(cn, entidadDeportivaId, codigo);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return existe;
        }

        public bool GuardarEntidadDeportivaMenu(EntidadDeportivaBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            bool seGuardo = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                SqlTransaction tran = null;
                try
                {
                    cn.Open();
                    if (registro != null)
                    {
                        if (registro.ListaMenu != null)
                        {
                            tran = cn.BeginTransaction();
                            foreach (MenuBE item in registro.ListaMenu)
                            {
                                EntidadDeportivaMenuBE value = new EntidadDeportivaMenuBE
                                {
                                    EntidadDeportivaId = registro.EntidadDeportivaId,
                                    MenuId = item.MenuId,
                                    FlagActivo = item.FlagActivo
                                };

                                seGuardo = entidadDeportivaMenuDA.GuardarEntidadDeportivaMenu(cn, value, usuarioIdModificacion, tran);

                                if (!seGuardo) break;
                            }

                            if (seGuardo) tran.Commit();
                            else tran.Rollback();
                        }
                    }
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public bool GuardarEntidadDeportivaPerfil(EntidadDeportivaBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            bool seGuardo = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                SqlTransaction tran = null;
                try
                {
                    cn.Open();
                    if (registro != null)
                    {
                        if (registro.ListaPerfil != null)
                        {
                            tran = cn.BeginTransaction();
                            foreach (PerfilBE item in registro.ListaPerfil)
                            {
                                EntidadDeportivaPerfilBE value = new EntidadDeportivaPerfilBE
                                {
                                    EntidadDeportivaId = registro.EntidadDeportivaId,
                                    PerfilId = item.PerfilId,
                                    FlagActivo = item.FlagActivo
                                };

                                seGuardo = entidadDeportivaPerfilDA.GuardarEntidadDeportivaPerfil(cn, value, usuarioIdModificacion, tran);

                                if (!seGuardo) break;
                            }

                            if (seGuardo) tran.Commit();
                            else tran.Rollback();
                        }
                    }
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public bool GuardarEntidadDeportivaPerfilMenu(EntidadDeportivaBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            bool seGuardo = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                SqlTransaction tran = null;
                try
                {
                    cn.Open();
                    if (registro != null)
                    {
                        if (registro.ListaPerfil != null)
                        {
                            tran = cn.BeginTransaction();
                            foreach (PerfilBE item in registro.ListaPerfil)
                            {
                                if(item.ListaMenu != null)
                                {
                                    foreach (MenuBE x in item.ListaMenu)
                                    {
                                        EntidadDeportivaPerfilMenuBE value = new EntidadDeportivaPerfilMenuBE
                                        {
                                            EntidadDeportivaId = registro.EntidadDeportivaId,
                                            PerfilId = item.PerfilId,
                                            MenuId = x.MenuId,
                                            FlagActivo = x.FlagActivo
                                        };

                                        seGuardo = entidadDeportivaPerfilMenuDA.GuardarEntidadDeportivaPerfilMenu(cn, value, usuarioIdModificacion, tran);

                                        if (!seGuardo) break;
                                    }
                                }
                            }

                            if (seGuardo) tran.Commit();
                            else tran.Rollback();
                        }
                    }
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }
    }
}
