using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    class SaltMine : ISaltMine
    {
        private SqlConnection dbConnection;

        public SaltMine()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[Constants.SaltPileSQLProvider].ConnectionString;
            dbConnection = new SqlConnection(connectionString);
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

        private void CloseConnection()
        {
            if (dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
            }
        }

        private SqlParameter newConfigParam(string paramName, string paramValue, int paramSize = 30)
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

        private SqlCommand newConfigNonQuery(string storedProcedureName, string section, string preference)
        {
            SqlCommand command = new SqlCommand(storedProcedureName, dbConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(newConfigParam(Constants.At_Section, section));
            command.Parameters.Add(newConfigParam(Constants.At_Preference, preference));
            return command;
        }

        public string GetConfig(string section, string preference)
        {
            string resultValue;

            OpenConnection();
            using (SqlCommand command = newConfigNonQuery(Constants.SP_GetConfig, section, preference))
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

                resultValue = (string)command.Parameters[Constants.At_Value].Value;
            }
            CloseConnection();
            return resultValue;
        }

        public void SetConfig(string section, string preference, string valueToStore)
        {
            OpenConnection();
            using (SqlCommand command = newConfigNonQuery(Constants.SP_UpdateConfig, section, preference))
            {
                command.Parameters.Add(newConfigParam(Constants.At_Value, valueToStore, 1000));

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }
    }
}
