using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SaltyLogistics.Model;

namespace SaltyLogistics.ViewModel
{
    public class SaltPiles : ISaltPiles
    {
        public IList<Accounts> AccountList { get; private set; }

        public decimal NetBalance
        {
            get => SumAccountList();
        }

        public SaltPiles()
        {
            AccountList = new List<Accounts>();

            AccountList.Add(new Accounts { IsActive = true, Account = "Checking", Balance = 12_569m });
            AccountList.Add(new Accounts { IsActive = false, Account = "Savings", Balance = 212.66m });
            AccountList.Add(new Accounts { IsActive = true, Account = "Money Market", Balance = 1_808.18m });
            AccountList.Add(new Accounts { IsActive = true, Account = "CD", Balance = 5_000m });
        }


        private decimal SumAccountList()
        {
            decimal sum = 0m;
            foreach (Accounts account in AccountList)
            {
                if (account.IsActive)
                {
                    sum += account.Balance;
                }
            }
            return sum;
        }
    }
}
