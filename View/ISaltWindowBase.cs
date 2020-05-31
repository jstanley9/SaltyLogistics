using SaltyLogistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.View
{
    public interface ISaltWindowBase
    {
        void CloseWindow();
        int GetHashCode();
    }
}
