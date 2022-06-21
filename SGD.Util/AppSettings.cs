using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Util
{
    public class AppSettings
    {
        public static T Get<T>(string key, object valueDefault = null)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            T value = default(T);
            if (string.IsNullOrWhiteSpace(appSetting))
            {
                if (valueDefault != null) value = (T)valueDefault;
                return value;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            value = (T)(converter.ConvertFromInvariantString(appSetting));
            return value;
        }
    }
}
