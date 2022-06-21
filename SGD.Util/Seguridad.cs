using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Util
{
    public class Seguridad
    {
        public class MD5
        {
            public static byte[] Encriptar(string texto)
            {
                MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
                byte[] bs = Encoding.UTF8.GetBytes(texto);
                bs = x.ComputeHash(bs);
                return bs;
            }
        }
    }
}
