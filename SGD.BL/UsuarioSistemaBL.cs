using SGD.BE;
using SGD.DA;
using SGD.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BL
{
    public class UsuarioSistemaBL : BaseBL
    {
        UsuarioSistemaDA usuarioSistemaDA = new UsuarioSistemaDA();
        UsuarioSistemaPerfilSistemaDA usuarioSistemaPerfilSistemaDA = new UsuarioSistemaPerfilSistemaDA();
        MenuSistemaDA menuSistemaDA = new MenuSistemaDA();
        PerfilSistemaDA perfilSistemaDA = new PerfilSistemaDA();

        TokenBL tokenBL = new TokenBL();
        PlantillaBL plantillaBL = new PlantillaBL();

        Correo correoUtil = new Correo();

        public UsuarioSistemaBE ObtenerUsuarioSistema(string usuarioId, out string mensajeError, bool conMenu = false)
        {
            UsuarioSistemaBE item = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = usuarioSistemaDA.ObtenerUsuarioSistema(cn, usuarioId);
                    if (item != null)
                    {
                        if (conMenu)
                        {
                            item.ListaPerfilSistema = perfilSistemaDA.ListarPerfilSistemaPorFlagActivoUsuario(cn, usuarioId, true);
                            string perfilesId = item.ListaPerfilSistema == null ? null : string.Join(",", item.ListaPerfilSistema.Select(x => x.PerfilId).ToArray());
                            item.ListaMenuSistema = menuSistemaDA.ListarMenuSistemaPorFlagActivoPerfiles(cn, perfilesId, true);
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

        private List<MenuSistemaBE> ListarMenuJerarquicaDesdeListaMenuGeneral(List<MenuSistemaBE> listaGeneral, int? menuIdPadre = null)
        {
            List<MenuSistemaBE> listaJerarquica = listaGeneral == null ? null : listaGeneral.Count(x => x.MenuIdPadre == menuIdPadre) == 0 ? null : listaGeneral.Where(x => x.MenuIdPadre == menuIdPadre).ToList();

            if(listaJerarquica != null)
            {
                foreach(MenuSistemaBE item in listaJerarquica)
                {
                    item.ListaSubMenu = ListarMenuJerarquicaDesdeListaMenuGeneral(listaGeneral, item.MenuId);
                }
            }

            return listaJerarquica;
        }

        public UsuarioSistemaBE ObtenerUsuarioSistemaPorCorreo(string correo)
        {
            UsuarioSistemaBE item = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = usuarioSistemaDA.ObtenerUsuarioSistemaPorCorreo(cn, correo);
                    cn.Close();
                }
                catch (SqlException ex) { }
                catch (Exception ex) { }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return item;
        }

        public bool GuardarContraseñaUsuarioSistema(UsuarioSistemaBE registro)
        {
            bool seGuardo = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seGuardo = usuarioSistemaDA.GuardarContraseñaUsuarioSistema(cn, registro);
                    cn.Close();
                }
                catch (SqlException ex) { }
                catch (Exception ex) { }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public bool ValidarUsuario(string usuarioId, string contraseña, out UsuarioSistemaBE usuarioSistema, out string mensajeError)
        {
            bool esValido = false;
            usuarioSistema = ObtenerUsuarioSistema(usuarioId, out mensajeError, conMenu: true);

            bool existeUsuario = usuarioSistema != null;

            if (!existeUsuario)
            {
                mensajeError = mensajeError ?? "Usuario no existe";
                return esValido;
            }

            byte[] contraseñaByte = Seguridad.MD5.Encriptar(contraseña);
            bool contraseñaValida = usuarioSistema.ContraseñaByte.SequenceEqual(contraseñaByte);

            if (!contraseñaValida)
            {
                mensajeError = "Contraseña es incorrecta";
                return esValido;
            }

            usuarioSistema.ContraseñaByte = null;
            esValido = existeUsuario && contraseñaValida;

            return esValido;
        }

        public bool ValidarUsuario(string usuarioId, byte[] contraseña, out UsuarioSistemaBE usuarioSistema, out string mensajeError)
        {
            bool esValido = false;
            usuarioSistema = ObtenerUsuarioSistema(usuarioId, out mensajeError);

            bool existeUsuario = usuarioSistema != null;

            if (!existeUsuario)
            {
                mensajeError = mensajeError ?? "Usuario no existe";
                return esValido;
            }

            bool contraseñaValida = usuarioSistema.ContraseñaByte.SequenceEqual(contraseña);

            if (!contraseñaValida)
            {
                mensajeError = "Contraseña es incorrecta";
                return esValido;
            }

            usuarioSistema.ContraseñaByte = null;
            esValido = existeUsuario && contraseñaValida;

            return esValido;
        }

        public bool EnviarCorreoRecuperacionContraseña(string correo, string urlBase, out string mensajeError)
        {
            bool seEnvioCorreo = false;
            mensajeError = null;

            UsuarioSistemaBE usuarioSistema = ObtenerUsuarioSistemaPorCorreo(correo);

            if(usuarioSistema == null)
            {
                mensajeError = $"El correo {correo} ingresado no se encuentro asociado a ninguna cuenta";
                return seEnvioCorreo;
            }

            TokenBE token = new TokenBE
            {
                EnumTipoToken = Enumeracion.TipoToken.RecuperacionContraseña,
                MilisegundosDuracion = AppSettings.Get<long>("token.milisegundosDuracion"),
                TokenUsuarioSistema = new TokenUsuarioSistemaBE
                {
                    UsuarioId = usuarioSistema.UsuarioId
                }
            };

            bool seGuardoToken = tokenBL.GuardarToken(token, out string tokenId, out DateTime? fechaCreacion);

            if (seGuardoToken)
            {
                int enumTipoPlantilla = (int)Enumeracion.TipoPlantilla.RecuperacionContraseña;
                PlantillaBE plantilla = plantillaBL.ObtenerPlantillaPorTipoPlantilla(enumTipoPlantilla);
                if (plantilla == null)
                {
                    mensajeError = "Ocurrió un error inesperado";
                    return seEnvioCorreo;
                }
                string asunto = "Cambiar contraseña";
                string usuarioIdBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuarioSistema.UsuarioId));
                string cuerpoMensaje = plantilla.Texto.Replace("[USUARIOID]", usuarioSistema.UsuarioId).Replace("[LINK]", $"{urlBase}/login/cambiar-contraseña/{tokenId}/{usuarioIdBase64}");
                string[] listaCorreoDestinatario = new string[] { correo };
                byte[] logoBytes = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\dist\img\SGDLogo.png"));
                object[][] listaImagenIncrustada = new object[][] { new object[] { "logo",  logoBytes} };
                seEnvioCorreo = correoUtil.Enviar(listaCorreoDestinatario, asunto, cuerpoMensaje, listaImagenIncrustada: listaImagenIncrustada);
            }
            else
            {
                mensajeError = "Ocurrió un error inesperado";
            }

            return seEnvioCorreo;
        }

        public bool ActualizarContraseña(string usuarioId, string contraseña, out string mensajeError)
        {
            bool seActualizo = false;

            UsuarioSistemaBE usuarioSistema = ObtenerUsuarioSistema(usuarioId, out mensajeError);

            if(usuarioSistema == null)
            {
                mensajeError = mensajeError ?? "Usuario no existe";
                return seActualizo;
            }

            usuarioSistema.Contraseña = contraseña;

            seActualizo = GuardarContraseñaUsuarioSistema(usuarioSistema);

            if (!seActualizo)
            {
                mensajeError = "No se pudo actualizar la contraseña";
            }

            return seActualizo;
        }

        public List<UsuarioSistemaBE> BuscarUsuarioSistema(string usuarioId, string correo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, out string mensajeError)
        {
            List<UsuarioSistemaBE> lista = null;
            totalRows = 0;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = usuarioSistemaDA.BuscarUsuarioSistema(cn, usuarioId, correo, pageNumber, pageSize, sortName, sortOrder, out totalRows);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public bool CambiarFlagActivoUsuarioSistema(string usuarioId, bool flagActivo, string usuarioIdModificacion, out string mensajeError)
        {
            bool seCambio = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seCambio = usuarioSistemaDA.CambiarFlagActivoUsuarioSistema(cn, usuarioId, flagActivo, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seCambio;
        }

        public bool ExisteIdUsuarioSistema(int tipoMantenimiento, string usuarioId, out string mensajeError)
        {
            bool existe = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    existe = usuarioSistemaDA.ExisteIdUsuarioSistema(cn, tipoMantenimiento, usuarioId);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return existe;
        }

        public bool ExisteCorreoUsuarioSistema(string usuarioId, string correo, out string mensajeError)
        {
            bool existe = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    existe = usuarioSistemaDA.ExisteCorreoUsuarioSistema(cn, usuarioId, correo);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return existe;
        }

        public bool GuardarUsuarioSistema(int tipoMantenimiento, UsuarioSistemaBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            bool seGuardo = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seGuardo = usuarioSistemaDA.GuardarUsuarioSistema(cn, tipoMantenimiento, registro, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public bool GuardarListaPerfilSistemaPorUsuario(UsuarioSistemaBE registro, string usuarioIdModificacion, out string mensajeError)
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
                        if (registro.ListaPerfilSistema != null)
                        {
                            tran = cn.BeginTransaction();
                            foreach (PerfilSistemaBE item in registro.ListaPerfilSistema)
                            {

                                UsuarioSistemaPerfilSistemaBE value = new UsuarioSistemaPerfilSistemaBE
                                {
                                    UsuarioSistemaId = registro.UsuarioId,
                                    PerfilSistemaId = item.PerfilId,
                                    FlagActivo = item.FlagActivo
                                };

                                seGuardo = usuarioSistemaPerfilSistemaDA.GuardarUsuarioSistemaPerfilSistema(cn, value, usuarioIdModificacion, tran);

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
    }
}
