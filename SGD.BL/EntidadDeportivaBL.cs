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

        public EntidadDeportivaBE ObtenerEntidadDeportiva(int entidadDeportivaId, out string mensajeError)
        {
            EntidadDeportivaBE item = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = entidadDeportivaDA.ObtenerEntidadDeportiva(cn, entidadDeportivaId);
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
    }
}
