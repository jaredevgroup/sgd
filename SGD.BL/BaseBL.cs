using SGD.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.BL
{
    public class BaseBL
    {
        static string cnName = AppSettings.Get<string>("cn.name");
        protected string cnString = ConfigurationManager.ConnectionStrings[cnName].ConnectionString;
    }
}
