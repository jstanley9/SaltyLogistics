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
using SaltyLogistics.Preferences;
using SaltyLogistics.Model;

namespace SaltyLogistics.Model 
{
    public class SaltMine : ISaltMine, IPreferencesIO
    {
        private SqlConnection dbConnection;
        
        private static readonly object padlock = new object(); 
        private Int64 lastId; 
//*********************************************************************************************************************
        public SaltMine()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[Constants.SaltPileSQLProvider].ConnectionString;
            dbConnection = new SqlConnection(connectionString);
        }
//*********************************************************************************************************************
        private void CloseConnection()
        {
            if (dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
            }
        }

        private Int64 CreateID()
        {
            Int64 id;
            lock (padlock) // Make thread safe
            {
                do
                {
                    DateTime dt = DateTime.Now;
                    string idString = dt.Year.ToString("D4")
                                    + dt.Month.ToString("D2")
                                    + dt.Day.ToString("D2")
                                    + dt.Hour.ToString("D2")
                                    + dt.Minute.ToString("D2")
                                    + dt.Second.ToString("D2")
                                    + dt.Millisecond.ToString("D3");
                    id = Int64.Parse(idString);
                } while (id == lastId);
                lastId = id;
            }
            return id;
        }

        public string GetConfig(string section, string preference)
        {
            string resultValue;

            OpenConnection();
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

                resultValue = ConvertFromDBVal<string>(command.Parameters[Constants.At_Value].Value);
            }
            CloseConnection();
            return resultValue;
        }

        private SqlCommand NewConfigNonQuery(string storedProcedureName, string section, string preference)
        {
            SqlCommand command = NewStoredProcedureCommand(storedProcedureName);

            command.Parameters.Add(NewConfigParam(Constants.At_Section, section));
            command.Parameters.Add(NewConfigParam(Constants.At_Preference, preference));
            return command;
        }

        private SqlCommand NewStoredProcedureCommand(string storedProcedureName)
        {
            SqlCommand command = new SqlCommand(storedProcedureName, dbConnection);
            command.CommandType = CommandType.StoredProcedure;
            return command;
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

        private void OpenConnection()
        {
            switch (dbConnection.State)
            {
                case ConnectionState.Closed:
                    {
                        dbConnection.Open();
                        break;
                    }
                case ConnectionState.Open:
                case ConnectionState.Connecting:
                case ConnectionState.Executing:
                case ConnectionState.Fetching:
                    {
                        break;
                    }
                case ConnectionState.Broken:
                    {
                        dbConnection.Close();
                        dbConnection.Open();
                        break;
                    }
                default:
                    {
                        throw new SaltDBException($"Unknown ConnectionState of {dbConnection.State}");
                    }

            }
        }

        public void SetConfig(string section, string preference, string valueToStore)
        {
            OpenConnection();
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
                CloseConnection();
            }
        }

        private static T ConvertFromDBVal<T>(object val)
        {
            if (val == DBNull.Value || val == null)
            {
                return default(T);
            }
            else
            {
                return (T)val;
            }
        }

        public void LoadAccountTypeList(List<AccountType> accountTypeList)
        {
            accountTypeList.Clear();
            OpenConnection();
            try
            {
                SqlCommand cmdSelect = NewStoredProcedureCommand(Constants.SP_GetAccountTypesActive);
                using (SqlDataReader dataReader = cmdSelect.ExecuteReader())
                {
                    foreach (IDataRecord dataRow in dataReader)
                    {
                        IAccountTypeDB accountType = new AccountType();
                        accountType.LoadFromDataRecord(dataRow);
                        accountTypeList.Add(accountType);
                    }
                }
                foreach (DataTableReader dataReader in )
            }
            finally
            {
                CloseConnection();
            }
        }

    }
}
