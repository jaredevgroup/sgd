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
    public class PerfilBL : BaseBL
    {
        PerfilDA perfilDA = new PerfilDA();
        //PerfilMenuDA perfilMenuDA = new PerfilMenuDA();

        public bool GuardarPerfil(PerfilBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            mensajeError = null;
            bool seGuardo = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seGuardo = perfilDA.GuardarPerfil(cn, registro, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public List<PerfilBE> BuscarPerfil(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, out string mensajeError)
        {
            List<PerfilBE> lista = null;
            totalRows = 0;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = perfilDA.BuscarPerfil(cn, nombre, pageNumber, pageSize, sortName, sortOrder, out totalRows);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public bool CambiarFlagActivoPerfil(int perfilId, string usuarioIdModificacion, bool flagActivo, out string mensajeError)
        {
            mensajeError = null;
            bool seCambio = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seCambio = perfilDA.CambiarFlagActivoPerfil(cn, perfilId, flagActivo, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seCambio;
        }

        public PerfilBE ObtenerPerfil(int perfilId, out string mensajeError)
        {
            PerfilBE item = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = perfilDA.ObtenerPerfil(cn, perfilId);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return item;
        }

        public bool ExisteNombrePerfil(int? perfilId, string nombre, out string mensajeError)
        {
            bool existe = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    existe = perfilDA.ExisteNombrePerfil(cn, perfilId, nombre);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return existe;
        }

        //public bool GuardarListaMenuPorPerfil(PerfilBE registro, string usuarioIdModificacion, out string mensajeError)
        //{
        //    mensajeError = null;
        //    bool seGuardo = false;

        //    using (SqlConnection cn = new SqlConnection(cnString))
        //    {
        //        SqlTransaction tran = null;
        //        try
        //        {
        //            cn.Open();
        //            if (registro != null)
        //            {
        //                if (registro.ListaMenu != null)
        //                {
        //                    tran = cn.BeginTransaction();
        //                    foreach (MenuBE item in registro.ListaMenu)
        //                    {

        //                        PerfilMenuBE value = new PerfilMenuBE
        //                        {
        //                            PerfilId = registro.PerfilId,
        //                            MenuId = item.MenuId,
        //                            FlagActivo = item.FlagActivo
        //                        };

        //                        seGuardo = perfilMenuDA.GuardarPerfilMenu(cn, value, usuarioIdModificacion, tran);

        //                        if (!seGuardo) break;
        //                    }

        //                    if (seGuardo) tran.Commit();
        //                    else tran.Rollback();
        //                }
        //            }
        //            cn.Close();
        //        }
        //        catch (SqlException ex) { mensajeError = ex.Message; }
        //        catch (Exception ex) { mensajeError = ex.Message; }
        //        finally
        //        {
        //            if (!seGuardo && tran != null) tran.Rollback();
        //            if (cn.State == ConnectionState.Open) cn.Close();
        //        }
        //    }

        //    return seGuardo;
        //}

        public List<PerfilBE> ListarPerfilPorFlagActivo(bool? flagActivo, out string mensajeError)
        {
            List<PerfilBE> lista = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = perfilDA.ListarPerfilPorFlagActivo(cn, flagActivo);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public List<PerfilBE> ListarPerfilPorFlagActivoUsuario(int entidadDeportivaId, string usuarioId, bool? flagActivo, out string mensajeError)
        {
            List<PerfilBE> lista = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = perfilDA.ListarPerfilPorFlagActivoUsuario(cn, entidadDeportivaId, usuarioId, flagActivo);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public List<PerfilBE> ListarPerfilPorFlagActivoEntidadDeportiva(int entidadDeportivaId, bool? flagActivo, out string mensajeError)
        {
            List<PerfilBE> lista = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = perfilDA.ListarPerfilPorFlagActivoEntidadDeportiva(cn, entidadDeportivaId, flagActivo);
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
