using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    public interface IAccountTypeDB
    {
        void AssignFromDBData(SqlDataReader readRow);
    }
}
