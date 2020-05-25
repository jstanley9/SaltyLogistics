using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SaltyLogistics.Model;

namespace SaltyLogistics.ViewModel
{
    public interface ISaltPiles
    {
        IList<Accounts> AccountList { get; }
        decimal NetBalance { get; }
        void Exit_Click(object sender, RoutedEventArgs eventArgs);
    }
}
