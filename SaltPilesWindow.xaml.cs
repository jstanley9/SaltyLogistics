using SaltyLogistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SaltyLogistics.ViewModel;

namespace SaltyLogistics
{
    /// <summary>
    /// Central window, all operations trigger off this window.
    /// This window is always up and will change as accounts are added, inactivated, activated, deleted, and changed.
    /// The other windows do not know about this window. Their actions will trigger update events for this window.
    /// </summary>
    public partial class SaltPilesWindow : Window
    {
        private ISaltPiles saltPiles;

        public SaltPilesWindow()
        {
            InitializeComponent();

            CoreModel core = CoreModel.GetCoreModel();
            saltPiles = core.SaltPiles;

            GridAccountList.ItemsSource = saltPiles.AccountList;
            LabelNetBalance.Content = saltPiles.NetBalance.ToString("C");
        }
    }
}
