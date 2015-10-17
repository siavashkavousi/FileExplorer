using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    static class Extensions
    {
        public static int ToInt32(this byte[] Bytes)
        {
            int x = 0;
            for (int i = Bytes.Length - 1; i >= 0; i--)
                x = x * 16 * 16 + (int)Bytes[i];
            return x;
        }
        public static string YesNo(this bool b)
        {
            return b ? "Yes" : "No";
        }
    }
}
