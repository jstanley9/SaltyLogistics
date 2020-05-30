using SaltyLogistics.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.DB
{
    public class DBSupport
    {
        private static readonly object padlock = new object();
        private static Int64 lastId;
        public SqlConnection Connection { get; }

        public DBSupport()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[Constants.SaltPileSQLProvider].ConnectionString;
            Connection = new SqlConnection(connectionString);
        }

        public static void AddParameterBigInt(SqlCommand command, string name, long value)
        {
            SqlParameter paramResult = new SqlParameter
            {
                ParameterName = name,
                SqlDbType = SqlDbType.BigInt,
                Direction = ParameterDirection.Input,
                Value = value
            };
            command.Parameters.Add(paramResult);
        }

        public static void AddParameterBit(SqlCommand command, string name, bool value)
        {
            SqlParameter paramResult = new SqlParameter
            {
                ParameterName = name,
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = value
            };
            command.Parameters.Add(paramResult);
        }

        public static void AddParameterFloat(SqlCommand command, string name, double value)
        {
            SqlParameter paramResult = new SqlParameter
            {
                ParameterName = name,
                SqlDbType = SqlDbType.Float,
                Direction = ParameterDirection.Input,
                Value = value
            };
            command.Parameters.Add(paramResult);
        }

        public static void AddParameterInt(SqlCommand command, string name, int value)
        {
            SqlParameter paramResult = new SqlParameter
            {
                ParameterName = name,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = value
            };
            command.Parameters.Add(paramResult);
        }

        public static void AddParameterNVarChar(SqlCommand command, string name, string value, int length)
        {
            SqlParameter paramResult = new SqlParameter
            {
                ParameterName = name,
                SqlDbType = SqlDbType.NVarChar,
                Size = length,
                Direction = ParameterDirection.Input
            };
            command.Parameters.Add(paramResult);
        }

        public void CloseConnection()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }

        public static T ConvertFromDBVal<T>(object val)
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

        public static  Int64 CreateID()
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

        public void OpenConnection()
        {
            switch (Connection.State)
            {
                case ConnectionState.Closed:
                    {
                        Connection.Open();
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
                        Connection.Close();
                        Connection.Open();
                        break;
                    }
                default:
                    {
                        throw new SaltDBException($"Unknown ConnectionState of {Connection.State}");
                    }
            }
        }
    }
}
