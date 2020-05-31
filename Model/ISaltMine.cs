using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    public interface ISaltMine
    {
        List<IAccountTypeClient> GetAccountTypesActive();
        string GetConfig(string section, string preference);
        Accounts SaveAccountDefinition(Accounts Account);
        void SetConfig(string section, string preference, string value);
        void UpdateAccountActiveStatus(long id, bool isActive);
    }
}
