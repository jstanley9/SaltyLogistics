using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    public class AccountType : IAccountTypeDB, IAccountTypeClient
    {
        public long Id { get; private set; }
        public string Name { get; set; }
        public bool IsAsset { get; set; }
        public bool IsInterestComputed { get; set; }
        public bool IsActive { get; private set; }

        void IAccountTypeDB.AssignFromDBData(SqlDataReader readRow)
        {
            Id = (long)readRow[Constants.AccountIdType];
            IsActive = (bool)readRow[Constants.IsActive];
            IsAsset = (bool)readRow[Constants.IsAsset];
            IsInterestComputed = (bool)readRow[Constants.IsInterestComputed];
            Name = (string)readRow[Constants.Name];
        }

        public void Activate()
        {
            IsActive = true;
        }
        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
