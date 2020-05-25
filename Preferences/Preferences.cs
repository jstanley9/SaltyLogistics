using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Preferences
{
    class Preferences : IPreferences
    {

        private IPreferencesIO IO;
        public Preferences(IPreferencesIO IOModule)
        {
            this.IO = IOModule;
        }

        public bool GetConfigBool(string section, string preference, bool defaultValue = default(bool))
        {
            string value = GetConfigString(section, preference);
            if (value == string.Empty || !(bool.TryParse(value, out bool result)))
            {
                return defaultValue;
            }
            else
            {
                return result;
            }
        }
        public decimal GetConfigCurrency(string section, string preference, decimal defaultValue = default(decimal))
        {
            string value = GetConfigString(section, preference);
            if (value == string.Empty || !decimal.TryParse(value, out decimal amount))
            {
                return defaultValue;
            }
            else
            {
                return amount;
            }
        }
        public double GetConfigDouble(string section, string preference, double defaultValue = default(double))
        {
            string value = GetConfigString(section, preference);
            if (value == string.Empty || !double.TryParse(value, out double amount))
            {
                return defaultValue;
            }
            else
            {
                return amount;
            }
        }
        public int GetConfigInt(string section, string preference, int defaultValue = default(int))
        {
            string value = GetConfigString(section, preference);
            if (value == string.Empty || !int.TryParse(value, out int amount))
            {
                return defaultValue;
            }
            else
            {
                return amount;
            }
        }
        public long GetConfigLong(string section, string preference, long defaultValue = default(long))
        {
            string value = GetConfigString(section, preference);
            if (value == string.Empty || !long.TryParse(value, out long amount))
            {
                return defaultValue;
            }
            else
            {
                return amount;
            }
        }
        public DateTime GetConfigDateTime(string section, string preference, DateTime defaultValue = default(DateTime))
        {
            string value = GetConfigString(section, preference);
            if (value == string.Empty || !DateTime.TryParse(value, out DateTime amount))
            {
                return defaultValue;
            }
            else
            {
                return amount;
            }
        }
        public string GetConfigString(string section, string preference, string defaultValue = "")
        {
            string value = IO.GetConfig(section, preference);
            if (value == default(string))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        public void SetConfigBool(string section, string preference, bool newValue)
        {
            string value = newValue ? Constants.True : Constants.False;
            SetConfigString(section, preference, value);
        }

        public void SetConfigCurrency(string section, string preference, decimal newValue)
        {
            SetConfigString(section, preference, newValue.ToString());
        }
        public void SetConfigDouble(string section, string preference, double newValue)
        {
            SetConfigString(section, preference, newValue.ToString());
        }
        public void SetConfigInt(string section, string preference, int newValue)
        {
            SetConfigString(section, preference, newValue.ToString());
        }
        public void SetConfigLong(string section, string preference, long newValue)
        {
            SetConfigString(section, preference, newValue.ToString());
        }
        public void SetConfigDateTime(string section, string preference, DateTime newValue)
        {
            SetConfigString(section, preference, newValue.ToString());
        }
        public void SetConfigString(string section, string preference, string newValue)
        {
            IO.SetConfig(section, preference, newValue);
        }
    }
}
