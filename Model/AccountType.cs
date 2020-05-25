using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    public class AccountType
    {
        public string AccountTypeId { get; private set; }
        private string AccountTypeNameId { get; set; }
        public string AccountTypeName { get; private set; }
        public bool IsAsset { get; set; }
        public bool IsInterestComputed { get; set; }
        public bool IsActive { get; private set; }
    }
}
