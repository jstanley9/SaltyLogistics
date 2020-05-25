using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    public class SaltException : Exception
    {
        public SaltException(string message) : base(message)
        {

        }
    }

    public class SaltDBException : SaltException
    {
        public SaltDBException(string message) : base(message)
        {

        }
    }

    public class SaltInvalidArgumentType : SaltException
    {
        public SaltInvalidArgumentType(string message) : base(message)
        {

        }
    }

}
