using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaltyLogistics.Model;

namespace SaltyLogistics.ViewModel
{
    public interface ISaltPiles
    {
        IList<Accounts> AccountList { get; }
        decimal NetBalance { get;  }
    }
}
