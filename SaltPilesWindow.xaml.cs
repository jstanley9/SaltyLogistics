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

namespace SaltyLogistics
{
    /// <summary>
    /// Central window, all operations trigger off this window.
    /// This window is always up and will change as accounts are added, inactivated, activated, deleted, and changed.
    /// The other windows do not know about this window. Their actions will trigger update events for this window.
    /// </summary>
    public partial class SaltPilesWindow : Window
    {
        private CoreModel core;
        public IList<Accounts> AccountList { get; private set; }

        public decimal NetBalance
        {
            get => SumAccountList();
        }

        private bool showAllAccounts;
        public bool ShowAllAccounts
        {
            get => showAllAccounts;
            set => SetShowAllAccounts(value);
        }

//*********************************************************************************************************************
        public SaltPilesWindow()
        {
            InitializeComponent();

            Init();
        }
//*********************************************************************************************************************
        private void Exit_Click(object sender, RoutedEventArgs eventArgs)
        {
            core.ShutDown();
            this.Close();
        }

        private void Init()
        {
            core = CoreModel.GetCoreModel();
            InitAccountList();

            GridAccountList.ItemsSource = AccountList;
            LabelNetBalance.Content = NetBalance.ToString("C");
        }

        private void InitAccountList()
        {
            AccountList = new List<Accounts>();
            LoadAccountList();
        }

        private void LoadAccountList()
        {
            AccountList.Add(new Accounts { IsActive = true, Name = "Checking", Balance = 12_569m });
            AccountList.Add(new Accounts { IsActive = false, Name = "Savings", Balance = 212.66m });
            AccountList.Add(new Accounts { IsActive = true, Name = "Money Market", Balance = 1_808.18m });
        }
        private void SetShowAllAccounts(bool newSetting)
        {
            if (showAllAccounts != newSetting)
            {
                showAllAccounts = newSetting;
                core.UpdateShowingAllAccounts(newSetting);
            }
        }

        private void CheckShowAllAccounts_Click(object sender, RoutedEventArgs e) => 
                     showAllAccounts = CheckShowAllAccounts.IsChecked ??  false;


        private decimal SumAccountList()
        {
            decimal sum = 0m;
            foreach (Accounts account in AccountList)
            {
                if (account.IsActive)
                {
                    sum += account.Balance;
                }
            }
            return sum;
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            core.ShowAccountMaintenance(OpenWindowAction.NewItem, AccountList);
        }
    }
}
