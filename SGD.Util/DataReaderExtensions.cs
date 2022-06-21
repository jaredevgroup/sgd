using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Util
{
    public static class DataReaderExtensions
    {
        public static T GetData<T>(this IDataReader dr, string field)
        {
            T value = default(T);

            if (!DBNull.Value.Equals(dr[field])) value = (T)dr[field];

            return value;
        }
    }
}
