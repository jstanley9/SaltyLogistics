using SaltyLogistics.DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    public class Accounts
    {
        public long Id { get; private set; }
        public string Name { get; set; }
        public string AccountTypeName { get; set; }
        private long AccountTypeId { get; set; }
        public decimal Balance { get; set; }
        public double InterestRate { get; set; }
        public string InterestRateToString { get => String.Format("0:0.0000", InterestRate); }
        public bool HasInterest { get => InterestRate != 0; }
        public bool IsActive { get; set; }
        public bool IsAsset { get; private set; }
        public int MonthsToKeep { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Accounts)
            {
                return Id.Equals((obj as Accounts).Id);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public Accounts InsertUpdateAccount(DBSupport db)
        {
            SqlCommand insertUpdate;
            if (Id == 0)
            {
                Id = DBSupport.CreateID();
                insertUpdate = new SqlCommand(Constants.SP_InsertAccounts, db.Connection);
            }
            else
            {
                insertUpdate = new SqlCommand(Constants.SP_UpdateAccounts, db.Connection);
            }
            DBSupport.AddParameterBigInt(insertUpdate, Constants.At_Id, Id);
            DBSupport.AddParameterNVarChar(insertUpdate, Constants.At_Name, Name, Constants.Name_Length);
            DBSupport.AddParameterBigInt(insertUpdate, Constants.At_AccountTypeId, AccountTypeId);
            DBSupport.AddParameterFloat(insertUpdate, Constants.At_InterestRate, InterestRate);
            DBSupport.AddParameterInt(insertUpdate, Constants.At_MonthsToKeep, MonthsToKeep);
            DBSupport.AddParameterBit(insertUpdate, Constants.At_IsActive, IsActive);

            db.OpenConnection();
            try
            {
                insertUpdate.ExecuteNonQuery();
            }
            finally
            {
                db.CloseConnection();
            }
            return this;
        }

        public void SetAccountType(IAccountTypeClient AccountType)
        {
            AccountTypeId = AccountType.Id;
            AccountTypeName = AccountType.Name;
        }
        public static void UpdateAccountActiveStatus(DBSupport db, long id, bool isActive)
        {
            using (SqlCommand updateIsActive = db.NewStoredProcedureCommand(Constants.SP_UpdateAccountActiveStatus))
            {
                DBSupport.AddParameterBigInt(updateIsActive, Constants.At_Id, id);
                DBSupport.AddParameterBit(updateIsActive, Constants.At_IsActive, isActive);

                db.OpenConnection();
                try
                {
                    updateIsActive.ExecuteNonQuery();
                }
                finally
                {
                    db.CloseConnection();
                }
            }
        }
    }
}
