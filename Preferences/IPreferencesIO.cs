using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Preferences
{
    interface IPreferencesIO
    {
        string GetConfig(string section, string preference);
        void SetConfig(string section, string preference, string valueToStore);
    }
}
