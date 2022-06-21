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
    public class PerfilSistemaBL : BaseBL
    {
        PerfilSistemaDA perfilDA = new PerfilSistemaDA();
        PerfilSistemaMenuSistemaDA perfilSistemaMenuSistemaDA = new PerfilSistemaMenuSistemaDA();

        public bool GuardarPerfilSistema(PerfilSistemaBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            mensajeError = null;
            bool seGuardo = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seGuardo = perfilDA.GuardarPerfilSistema(cn, registro, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public List<PerfilSistemaBE> BuscarPerfilSistema(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, out string mensajeError)
        {
            List<PerfilSistemaBE> lista = null;
            totalRows = 0;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = perfilDA.BuscarPerfilSistema(cn, nombre, pageNumber, pageSize, sortName, sortOrder, out totalRows);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public bool CambiarFlagActivoPerfilSistema(int perfilId, string usuarioIdModificacion, bool flagActivo, out string mensajeError)
        {
            mensajeError = null;
            bool seCambio = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seCambio = perfilDA.CambiarFlagActivoPerfilSistema(cn, perfilId, flagActivo, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seCambio;
        }

        public PerfilSistemaBE ObtenerPerfilSistema(int perfilId, out string mensajeError)
        {
            PerfilSistemaBE item = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = perfilDA.ObtenerPerfilSistema(cn, perfilId);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return item;
        }

        public bool ExisteNombrePerfilSistema(int? perfilId, string nombre, out string mensajeError)
        {
            bool existe = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    existe = perfilDA.ExisteNombrePerfilSistema(cn, perfilId, nombre);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return existe;
        }

        public bool GuardarListaMenuSistemaPorPerfil(PerfilSistemaBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            mensajeError = null;
            bool seGuardo = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                SqlTransaction tran = null;
                try
                {
                    cn.Open();
                    if (registro != null)
                    {
                        if (registro.ListaMenuSistema != null)
                        {
                            tran = cn.BeginTransaction();
                            foreach (MenuSistemaBE item in registro.ListaMenuSistema)
                            {

                                PerfilSistemaMenuSistemaBE value = new PerfilSistemaMenuSistemaBE
                                {
                                    PerfilId = registro.PerfilId,
                                    MenuId = item.MenuId,
                                    FlagActivo = item.FlagActivo
                                };

                                seGuardo = perfilSistemaMenuSistemaDA.GuardarPerfilSistemaMenuSistema(cn, value, usuarioIdModificacion, tran);

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
                finally
                {
                    if (!seGuardo && tran != null) tran.Rollback();
                    if (cn.State == ConnectionState.Open) cn.Close();
                }
            }

            return seGuardo;
        }

        public List<PerfilSistemaBE> ListarPerfilSistemaPorFlagActivo(bool? flagActivo, out string mensajeError)
        {
            List<PerfilSistemaBE> lista = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = perfilDA.ListarPerfilSistemaPorFlagActivo(cn, flagActivo);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public List<PerfilSistemaBE> ListarPerfilSistemaPorFlagActivoUsuario(string usuarioId, bool? flagActivo, out string mensajeError)
        {
            List<PerfilSistemaBE> lista = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = perfilDA.ListarPerfilSistemaPorFlagActivoUsuario(cn, usuarioId, flagActivo);
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
