using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Utilities
{
    public static class StringUtilities
    {
        public static int PreviousIndexOf(this string str, char predicate, int index)
        {
                if (index > str.Length - 1) index = str.Length - 1;
                for (int i = index; i > 0; i--)
                {

                        if (str[i] == predicate) return i-2;
                }
            
            
            return -1;
        }
    }
}
