using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Utils
{
    class StringUtilities
    {
        public static bool StringHasData(string Value)
        {
            return (!(String.IsNullOrEmpty(Value) || String.IsNullOrEmpty(Value.Trim())));
        }
    }
}
