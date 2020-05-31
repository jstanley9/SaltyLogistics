using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SaltyLogistics.DB;
using SaltyLogistics.Preferences;
using SaltyLogistics.Model;

namespace SaltyLogistics.Model 
{
    public class SaltMine : ISaltMine, IPreferencesIO
    {
        private DBSupport db;    
        
//*********************************************************************************************************************
        public SaltMine()
        {
            db = new DBSupport();
        }
//*********************************************************************************************************************
        public List<IAccountTypeClient> GetAccountTypesActive()
        {
            List<IAccountTypeClient> accountTypeList = new List<IAccountTypeClient>();
            using (SqlCommand select = new SqlCommand(Constants.GetAccounttypesActive, db.Connection))
            {
                select.CommandType = CommandType.StoredProcedure;
                db.OpenConnection();
                try
                {

                    using (SqlDataReader readRow = select.ExecuteReader())
                    {
                        while (readRow.Read())
                        {
                            IAccountTypeDB accountType = new AccountType();
                            accountType.AssignFromDBData(readRow);
                            accountTypeList.Add(accountType as IAccountTypeClient);
                        }
                    }
                }
                finally
                {
                    db.CloseConnection();
                }
            }
            return accountTypeList;
        }

        public string GetConfig(string section, string preference)
        {
            string resultValue;

            db.OpenConnection();
            using (SqlCommand command = NewConfigNonQuery(Constants.SP_GetConfig, section, preference))
            {
                SqlParameter paramResult = new SqlParameter
                {
                    ParameterName = Constants.At_Value,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 1000,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(paramResult);

                command.ExecuteNonQuery();

                resultValue = DBSupport.ConvertFromDBVal<string>(command.Parameters[Constants.At_Value].Value);
            }
            db.CloseConnection();
            return resultValue;
        }

        private SqlParameter NewConfigParam(string paramName, string paramValue, int paramSize = 30)
        {
            return new SqlParameter
            {
                ParameterName = paramName,
                SqlDbType = SqlDbType.NVarChar,
                Size = paramSize,
                Value = paramValue,
                Direction = ParameterDirection.Input
            };
        }

        private SqlCommand NewConfigNonQuery(string storedProcedureName, string section, string preference)
        {
            SqlCommand command = db.NewStoredProcedureCommand(storedProcedureName);

            command.Parameters.Add(NewConfigParam(Constants.At_Section, section));
            command.Parameters.Add(NewConfigParam(Constants.At_Preference, preference));
            return command;
        }

        public Accounts SaveAccountDefinition(Accounts Accounts)
        {
            Accounts.InsertUpdateAccount(db);
            return Accounts;
        }

        public void SetConfig(string section, string preference, string valueToStore)
        {
            db.OpenConnection();
            try
            {
                using (SqlCommand command = NewConfigNonQuery(Constants.SP_UpdateConfig, section, preference))
                {
                    command.Parameters.Add(NewConfigParam(Constants.At_Value, valueToStore, 1000));
                        
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                db.CloseConnection();
            }
        }
        public void UpdateAccountActiveStatus(long id, bool isActive)
        {
            Accounts.UpdateAccountActiveStatus(db, id, isActive);
        }
    }
}
