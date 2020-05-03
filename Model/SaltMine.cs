using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    class SaltMine : ISaltMine
    {
        private DbConnection dbConnection;

        public SaltMine()
        {
            string dataProvider = ConfigurationManager.AppSettings["provider"];
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(dataProvider);

            dbConnection = dbFactory.CreateConnection();
            if (dbConnection == null)
            {
                throw new SaltDBException($"Failed to create a connection to the Salt database\nProvider='{dataProvider}'");
            }

            string connectionString = ConfigurationManager.ConnectionStrings["SaltPileSQLProvider"].ConnectionString;
            dbConnection.ConnectionString = connectionString;
        }
    }
}
