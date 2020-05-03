using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    interface ISaltMine
    {
        string GetConfig(string section, string preference);
        void SetConfig(string section, string preference, string value);
    }
}
