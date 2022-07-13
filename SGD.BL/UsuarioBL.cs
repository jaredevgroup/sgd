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
    public class UsuarioBL : BaseBL
    {

        UsuarioDA usuarioDA = new UsuarioDA();
        UsuarioPerfilDA usuarioPerfilDA = new UsuarioPerfilDA();
        MenuDA menuDA = new MenuDA();
        //PerfilDA perfilDA = new PerfilDA();

        TokenBL tokenBL = new TokenBL();
        PlantillaBL plantillaBL = new PlantillaBL();

        Correo correoUtil = new Correo();

        public UsuarioBE ObtenerUsuario(int entidadDeportivaId, string usuarioId, out string mensajeError, bool conMenu = false)
        {
            UsuarioBE item = null;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = usuarioDA.ObtenerUsuario(cn, entidadDeportivaId, usuarioId);
                    if (item != null)
                    {
                        if (conMenu)
                        {
                            //item.ListaPerfil = perfilDA.ListarPerfilPorFlagActivoUsuario(cn, usuarioId, true);
                            string perfilesId = item.ListaPerfil == null ? null : string.Join(",", item.ListaPerfil.Select(x => x.PerfilId).ToArray());
                            //item.ListaMenu = menuDA.ListarMenuPorFlagActivoPerfiles(cn, perfilesId, true);
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

        private List<MenuBE> ListarMenuJerarquicaDesdeListaMenuGeneral(List<MenuBE> listaGeneral, int? menuIdPadre = null)
        {
            List<MenuBE> listaJerarquica = listaGeneral == null ? null : listaGeneral.Count(x => x.MenuIdPadre == menuIdPadre) == 0 ? null : listaGeneral.Where(x => x.MenuIdPadre == menuIdPadre).ToList();

            if (listaJerarquica != null)
            {
                foreach (MenuBE item in listaJerarquica)
                {
                    item.ListaSubMenu = ListarMenuJerarquicaDesdeListaMenuGeneral(listaGeneral, item.MenuId);
                }
            }

            return listaJerarquica;
        }

        public UsuarioBE ObtenerUsuarioPorCorreo(int entidadDeportivaId, string correo)
        {
            UsuarioBE item = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    item = usuarioDA.ObtenerUsuarioPorCorreo(cn, entidadDeportivaId, correo);
                    cn.Close();
                }
                catch (SqlException ex) { }
                catch (Exception ex) { }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return item;
        }

        public bool GuardarContraseñaUsuario(UsuarioBE registro)
        {
            bool seGuardo = false;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seGuardo = usuarioDA.GuardarContraseñaUsuario(cn, registro);
                    cn.Close();
                }
                catch (SqlException ex) { }
                catch (Exception ex) { }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public bool ValidarUsuario(int entidadDeportivaId, string usuarioId, string contraseña, out UsuarioBE usuario, out string mensajeError)
        {
            bool esValido = false;
            usuario = ObtenerUsuario(entidadDeportivaId, usuarioId, out mensajeError, conMenu: true);

            bool existeUsuario = usuario != null;

            if (!existeUsuario)
            {
                mensajeError = mensajeError ?? "Usuario no existe";
                return esValido;
            }

            byte[] contraseñaByte = Seguridad.MD5.Encriptar(contraseña);
            bool contraseñaValida = usuario.ContraseñaByte.SequenceEqual(contraseñaByte);

            if (!contraseñaValida)
            {
                mensajeError = "Contraseña es incorrecta";
                return esValido;
            }

            usuario.ContraseñaByte = null;
            esValido = existeUsuario && contraseñaValida;

            return esValido;
        }

        public bool ValidarUsuario(int entidadDeportivaId, string usuarioId, byte[] contraseña, out UsuarioBE usuario, out string mensajeError)
        {
            bool esValido = false;
            usuario = ObtenerUsuario(entidadDeportivaId, usuarioId, out mensajeError);

            bool existeUsuario = usuario != null;

            if (!existeUsuario)
            {
                mensajeError = mensajeError ?? "Usuario no existe";
                return esValido;
            }

            bool contraseñaValida = usuario.ContraseñaByte.SequenceEqual(contraseña);

            if (!contraseñaValida)
            {
                mensajeError = "Contraseña es incorrecta";
                return esValido;
            }

            usuario.ContraseñaByte = null;
            esValido = existeUsuario && contraseñaValida;

            return esValido;
        }

        public bool EnviarCorreoRecuperacionContraseña(int entidadDeportivaId, string correo, string urlBase, out string mensajeError)
        {
            bool seEnvioCorreo = false;
            mensajeError = null;

            UsuarioBE usuario = ObtenerUsuarioPorCorreo(entidadDeportivaId, correo);

            if (usuario == null)
            {
                mensajeError = $"El correo {correo} ingresado no se encuentro asociado a ninguna cuenta";
                return seEnvioCorreo;
            }

            TokenBE token = new TokenBE
            {
                EnumTipoToken = Enumeracion.TipoToken.RecuperacionContraseña,
                MilisegundosDuracion = AppSettings.Get<long>("token.milisegundosDuracion"),
                TokenUsuario = new TokenUsuarioBE
                {
                    UsuarioId = usuario.UsuarioId
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
                string usuarioIdBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuario.UsuarioId));
                string cuerpoMensaje = plantilla.Texto.Replace("[USUARIOID]", usuario.UsuarioId).Replace("[LINK]", $"{urlBase}/login/cambiar-contraseña/{tokenId}/{usuarioIdBase64}");
                string[] listaCorreoDestinatario = new string[] { correo };
                byte[] logoBytes = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\dist\img\SGDLogo.png"));
                object[][] listaImagenIncrustada = new object[][] { new object[] { "logo", logoBytes } };
                seEnvioCorreo = correoUtil.Enviar(listaCorreoDestinatario, asunto, cuerpoMensaje, listaImagenIncrustada: listaImagenIncrustada);
            }
            else
            {
                mensajeError = "Ocurrió un error inesperado";
            }

            return seEnvioCorreo;
        }

        public bool ActualizarContraseña(int entidadDeportivaId, string usuarioId, string contraseña, out string mensajeError)
        {
            bool seActualizo = false;

            UsuarioBE usuario = ObtenerUsuario(entidadDeportivaId, usuarioId, out mensajeError);

            if (usuario == null)
            {
                mensajeError = mensajeError ?? "Usuario no existe";
                return seActualizo;
            }

            usuario.Contraseña = contraseña;

            seActualizo = GuardarContraseñaUsuario(usuario);

            if (!seActualizo)
            {
                mensajeError = "No se pudo actualizar la contraseña";
            }

            return seActualizo;
        }

        public List<UsuarioBE> BuscarUsuario(int entidadDeportivaId, string usuarioId, string correo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, out string mensajeError)
        {
            List<UsuarioBE> lista = null;
            totalRows = 0;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    lista = usuarioDA.BuscarUsuario(cn, entidadDeportivaId, usuarioId, correo, pageNumber, pageSize, sortName, sortOrder, out totalRows);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return lista;
        }

        public bool CambiarFlagActivoUsuario(int entidadDeportivaId, string usuarioId, bool flagActivo, string usuarioIdModificacion, out string mensajeError)
        {
            bool seCambio = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seCambio = usuarioDA.CambiarFlagActivoUsuario(cn, entidadDeportivaId, usuarioId, flagActivo, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seCambio;
        }

        public bool ExisteIdUsuario(int tipoMantenimiento, int entidadDeportivaId, string usuarioId, out string mensajeError)
        {
            bool existe = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    existe = usuarioDA.ExisteIdUsuario(cn, entidadDeportivaId, tipoMantenimiento, usuarioId);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return existe;
        }

        public bool ExisteCorreoUsuario(int entidadDeportivaId, string usuarioId, string correo, out string mensajeError)
        {
            bool existe = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    existe = usuarioDA.ExisteCorreoUsuario(cn, entidadDeportivaId, usuarioId, correo);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return existe;
        }

        public bool GuardarUsuario(int tipoMantenimiento, UsuarioBE registro, string usuarioIdModificacion, out string mensajeError)
        {
            bool seGuardo = false;
            mensajeError = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                try
                {
                    cn.Open();
                    seGuardo = usuarioDA.GuardarUsuario(cn, tipoMantenimiento, registro, usuarioIdModificacion);
                    cn.Close();
                }
                catch (SqlException ex) { mensajeError = ex.Message; }
                catch (Exception ex) { mensajeError = ex.Message; }
                finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            }

            return seGuardo;
        }

        public bool GuardarListaPerfilPorUsuario(UsuarioBE registro, string usuarioIdModificacion, out string mensajeError)
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
                        if (registro.ListaPerfil != null)
                        {
                            tran = cn.BeginTransaction();
                            foreach (PerfilBE item in registro.ListaPerfil)
                            {

                                UsuarioPerfilBE value = new UsuarioPerfilBE
                                {
                                    EntidadDeportivaId = registro.EntidadDeportivaId,
                                    UsuarioId = registro.UsuarioId,
                                    PerfilId = item.PerfilId,
                                    FlagActivo = item.FlagActivo
                                };

                                seGuardo = usuarioPerfilDA.GuardarUsuarioPerfil(cn, value, usuarioIdModificacion, tran);

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
