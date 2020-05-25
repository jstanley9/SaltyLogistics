using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Preferences
{
    interface IPreferences
    {
        bool GetConfigBool(string section, string preference, bool defaultValue = default(bool));
        decimal GetConfigCurrency(string section, string preference, decimal defaultValue = default(decimal));
        double GetConfigDouble(string section, string preference, double defaultValue = default(double));
        int GetConfigInt(string section, string preference, int defaultValue = default(int));
        long GetConfigLong(string section, string preference, long defaultValue = default(long));
        DateTime GetConfigDateTime(string section, string preference, DateTime defaultValue = default(DateTime));
        string GetConfigString(string section, string preference, string defaultValue = "");

        void SetConfigBool(string section, string preference, bool newValue);
        void SetConfigCurrency(string section, string preference, decimal newValue);
        void SetConfigDouble(string section, string preference, double newValue);
        void SetConfigInt(string section, string preference, int newValue);
        void SetConfigLong(string section, string preference, long newValue);
        void SetConfigDateTime(string section, string preference, DateTime newValue);
        void SetConfigString(string section, string preference, string newValue);
    }
}
