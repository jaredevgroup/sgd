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
    public class TokenBL : BaseBL
    {
        TokenDA tokenDA = new TokenDA();
        TokenUsuarioDA tokenUsuarioDA = new TokenUsuarioDA();
        TokenUsuarioSistemaDA tokenUsuarioSistemaDA = new TokenUsuarioSistemaDA();

        public bool GuardarToken(TokenBE registro, out string tokenId, out DateTime? fechaCreacion)
        {
            bool seGuardo = false;
            tokenId = null;
            fechaCreacion = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                SqlTransaction tran = null;
                try
                {
                    cn.Open();
                    tran = cn.BeginTransaction();

                    seGuardo = tokenDA.GuardarToken(cn, registro, out tokenId, out fechaCreacion);

                    if (seGuardo && registro.TokenUsuarioSistema != null)
                    {
                        registro.TokenUsuarioSistema.TokenId = tokenId;
                        seGuardo = tokenUsuarioSistemaDA.GuardarTokenUsuarioSistema(cn, registro.TokenUsuarioSistema, tran: tran);
                    }

                    if (seGuardo && registro.TokenUsuario != null)
                    {

                        registro.TokenUsuario.TokenId = tokenId;
                        seGuardo = tokenUsuarioDA.GuardarTokenUsuario(cn, registro.TokenUsuario, tran: tran);
                    }

                    if (seGuardo) tran.Commit();
                    else tran.Rollback();

                    cn.Close();
                }
                catch (SqlException ex) { }
                catch (Exception ex) { }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        //public bool GuardarToken(TokenBE registro, out string tokenId, out DateTime? fechaCreacion, bool conUsuarioSistema = false, string usuarioId = null)
        //{
        //    bool seGuardo = false;
        //    tokenId = null;
        //    fechaCreacion = null;

        //    using (SqlConnection cn = new SqlConnection(cnString))
        //    {
        //        SqlTransaction tran = null;
        //        try
        //        {
        //            cn.Open();
        //            tran = cn.BeginTransaction();

        //            bool seGuardoToken = tokenDA.GuardarToken(cn, registro, out tokenId, out fechaCreacion, tran: tran);
        //            if (seGuardoToken)
        //            {
        //                TokenUsuarioSistemaBE tokenUsuarioSistema = new TokenUsuarioSistemaBE
        //                {
        //                    TokenId = tokenId,
        //                    UsuarioId = usuarioId
        //                };

        //                seGuardo = tokenUsuarioSistemaDA.GuardarTokenUsuarioSistema(cn, tokenUsuarioSistema, tran: tran);
        //            }

        //            if (seGuardo) tran.Commit();
        //            else tran.Rollback();

        //            cn.Close();
        //        }
        //        catch (SqlException ex) { }
        //        catch (Exception ex) { }
        //        finally
        //        {
        //            if (!seGuardo && tran != null) tran.Rollback();
        //            if (cn.State == ConnectionState.Open) cn.Close();
        //        }
        //    }

        //    return seGuardo;
        //}

        public TokenBE ObtenerTokenPorUsuarioSistema(string tokenId, string usuarioId)
        {
            TokenBE item = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = tokenDA.ObtenerTokenPorUsuarioSistema(cn, tokenId, usuarioId);
                    cn.Close();
                }
                catch (SqlException ex) { }
                catch (Exception ex) { }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return item;
        }

        public bool ValidarTokenPorUsuarioSistema(string tokenId, string usuarioId, out string mensajeError)
        {
            bool esValido = false;
            mensajeError = null;

            TokenBE token = ObtenerTokenPorUsuarioSistema(tokenId, usuarioId);

            if (token == null)
            {
                mensajeError = "El token no es válido";
                return esValido;
            }

            DateTime fechaCaducidad = token.ObtenerFechaCaducidad();

            esValido = DateTime.Now < fechaCaducidad;

            if (!esValido)
            {
                mensajeError = $"El cambio de contraseña expiraba el {fechaCaducidad:dd-MM-yyyy} a las {fechaCaducidad:hh:mm:ss}";
                return esValido;
            }

            return esValido;
        }
    }
}
