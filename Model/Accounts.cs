using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    public partial class Accounts
    {
        public string AccountId { get; private set; }
        public string AccountName { get; set; }
        public string AccountTypeName { get; set; }
        private string AccountTypeId { get; set; }
        public decimal Balance { get; set; }
        public string Id { get; set; }
        public float IntrestRate { get; set; }
        public bool IsActive { get; set; }
        public bool IsAsset { get; private set; }
        public int MonthsToKeep { get; set; }

        public override int GetHashCode()
        {
            return AccountId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Accounts)
            {
                return AccountId.Equals((obj as Accounts).AccountId);
            }
            else
            {
                return false;
            }
        }
    }
}
