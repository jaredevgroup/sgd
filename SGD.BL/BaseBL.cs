using SGD.BE;
using SGD.DA;
using SGD.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BL
{
    public class BaseBL
    {
        static string cnName = AppSettings.Get<string>("cn.name");
        protected string cnString = ConfigurationManager.ConnectionStrings[cnName].ConnectionString;

        MenuDA menuDA = new MenuDA();

        protected List<MenuBE> ListarMenuPorFlagActivoMenuIdPadreEntidadDeportivaConSubMenu(SqlConnection cn, int entidadDeportivaId, MenuBE item, List<MenuBE> lista, bool? flagActivo)
        {
            if (item != null)
            {
                item.ListaSubMenu = menuDA.ListarMenuPorFlagActivoMenuIdPadreEntidadDeportiva(cn, entidadDeportivaId, flagActivo, item.MenuId);
                lista = item.ListaSubMenu;
            }

            if (lista != null)
            {
                foreach (MenuBE x in lista)
                {
                    x.ListaSubMenu = ListarMenuPorFlagActivoMenuIdPadreEntidadDeportivaConSubMenu(cn, entidadDeportivaId, x, x.ListaSubMenu, flagActivo);
                }
            }

            return lista;
        }

        protected List<MenuBE> ListarMenuPorFlagActivoMenuIdPadreConSubMenu(SqlConnection cn, MenuBE item, List<MenuBE> lista, bool? flagActivo)
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
    }
}
