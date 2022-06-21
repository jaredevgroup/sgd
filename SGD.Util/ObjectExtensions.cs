using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Util
{
    public static class ObjectExtensions
    {
        public static object GetNullable(this object obj)
        {
            object value = obj;

            if (obj == null) value = DBNull.Value;

            return value;
        }
    }
}
