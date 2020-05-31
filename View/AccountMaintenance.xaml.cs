using SaltyLogistics.Model;
using SaltyLogistics.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace SaltyLogistics.View
{
    /// <summary>
    /// Interaction logic for AccountMaintenance.xaml
    /// </summary>
    public partial class AccountMaintenance : Window, ISaltWindowBase
    {
        private List<Accounts> accountList;
        private List<IAccountTypeClient> accountTypes;
        private readonly CoreModel core;
        private Accounts currentAccount;
        private readonly Brush defaultColor;
        private bool isModified = false;
        private MaintType maintType = MaintType.NewAccount;

        private enum MaintType { ReadOnly, NewAccount, EditAccount }
//=====================================================================================================================
        public AccountMaintenance()
        {
            InitializeComponent();

            defaultColor = TextInterestRate.Foreground;
            core = CoreModel.GetCoreModel();
        }
//=====================================================================================================================
        public void CloseWindow()
        {
            core.WindowIsClosing(this);
            Close();
        }

        public void InitWindow(OpenWindowAction Action, object Info, int ItemIndex = -1)
        {
            if (Info != null)
            {
                accountList = Info as List<Accounts>;
            }
            else
            {
                accountList = new List<Accounts>();
            }

            switch (Action)
            {
                case OpenWindowAction.NewItem:
                    InitializeNewAccount();
                    break;
                case OpenWindowAction.ViewItem:
                    InitializeViewAccount(ItemIndex);
                    break;
                case OpenWindowAction.EditItem:
                    InitializeEditAccount(ItemIndex);
                    break;
                default:
                    throw new InvalidEnumArgumentException("Unknown Open Window Action {Action}");
            }
            isModified = false;
            accountTypes = core.LoadComboAccountType(ComboAccountType);
            Scatter();
        }
        
        private void InitializeNewAccount()
        {
            currentAccount = new Accounts();
            Title = Constants.AddAccount;
            maintType = MaintType.NewAccount;
        }
        
        private void InitializeViewAccount(int ItemIndex)
        {
            SetAccount(ItemIndex, MaintType.ReadOnly, Constants.EditAccount);
        }
       
        private void InitializeEditAccount(int ItemIndex)
        {
            SetAccount(ItemIndex, MaintType.EditAccount, Constants.ViewAccount);
        }

        private void SetAccount(int ItemIndex, MaintType MType, string NewTitle)
        {
            if (ItemIndex < 0)
            {
                InitializeNewAccount();
            }
            else 
            {
                currentAccount = accountList[ItemIndex];
                Title = NewTitle + currentAccount.Name;
                maintType = MType;
            }
        }

        private void Scatter()
        {
            if (maintType == MaintType.ReadOnly)
            {
                ChangeButtons.Visibility = Visibility.Hidden;
                Cancel.Visibility = Visibility.Hidden;
                SaveAndClose.Content = Constants.Save;
                TextAccountName.IsReadOnly = true;
                ComboAccountType.Visibility = Visibility.Hidden;
                ButtonNewType.Visibility = Visibility.Hidden;
                DisplayAccountType.Visibility = Visibility.Visible;
                TextInterestRate.IsReadOnly = true;
                TextMonthsToKeep.IsReadOnly = true;
            }
            else
            {
                ChangeButtons.Visibility = Visibility.Visible;
                Cancel.Visibility = Visibility.Visible;
                SaveAndClose.Content = Constants.SaveAndClose;
                TextAccountName.IsReadOnly = false;
                ComboAccountType.Visibility = Visibility.Visible;
                ButtonNewType.Visibility = Visibility.Visible;
                DisplayAccountType.Visibility = Visibility.Hidden;
                TextInterestRate.IsReadOnly = false;
                TextMonthsToKeep.IsReadOnly = false;
            }
            UpdateSuspendAccountButton();
            SaveAccount.IsEnabled = isModified;

            AccountChanged.Content = isModified ? Constants.Changed : String.Empty;

            DisplayAccountId.Content = currentAccount.Id;
            TextAccountName.Text = currentAccount.Name;
            int accountTypeIx = ComboAccountType.Items.IndexOf(currentAccount.AccountTypeName);
            ComboAccountType.SelectedIndex = accountTypeIx;
            DisplayAccountType.Content = currentAccount.AccountTypeName;
            if (accountTypes[accountTypeIx].IsAsset)
            {
                ComboAccountType.Foreground = Brushes.DarkGreen;
            }
            else
            {
                ComboAccountType.Foreground = Brushes.DarkRed;
            }
            TextInterestRate.Text = currentAccount.InterestRateToString;
            TextMonthsToKeep.Text = currentAccount.MonthsToKeep.ToString();
            if (currentAccount.IsActive)
            {
                DisplayActive.Content = Constants.StatusActive;
            }
            else
            {
                DisplayActive.Content = Constants.StatusSuspended;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AccountHasBeenModified();
            if (!isModified)
            {
                isModified = true;
                AccountChanged.Content = Constants.Changed;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccountHasBeenModified();
        }

        private void TextInterestRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            AccountHasBeenModified();
            if (double.TryParse(TextInterestRate.Text.Trim(), out _))
            {
                TextInterestRate.Foreground = defaultColor;
            }
            else
            {
                TextInterestRate.Foreground = Brushes.Red;
            }
        }

        private void AccountHasBeenModified()
        {
            if (!isModified)
            {
                isModified = true;
                SaveAccount.IsEnabled = true;
                AccountChanged.Content = Constants.Changed;
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void SaveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (isModified)
            {
                if (StringUtilities.StringHasData(TextAccountName.Text))
                {
                    SaveAndShow();
                }
                else
                {
                    MessageBox.Show(Constants.AnAccountMustHaveAName, Constants.AppName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                
            }
        }

        private void SaveAndShow()
        {
            if (Gather())
            {
                Save();
                if (maintType == MaintType.NewAccount)
                {
                    accountList.Add(currentAccount);
                    maintType = MaintType.EditAccount;
                    Title = Title = Constants.EditAccount + currentAccount.Name;
                }
                Scatter();
            }
        }

        private bool Gather()
        {
            bool result = true;

            double interestRate = 0;
            if (!(String.IsNullOrEmpty(TextInterestRate.Text) || double.TryParse(TextInterestRate.Text, out interestRate)))
            {
                result = false;
                MessageBox.Show(Constants.InvalidInterestRate, Constants.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            int monthsToKeep = 0;
            if (!(String.IsNullOrEmpty(TextMonthsToKeep.Text) || int.TryParse(TextMonthsToKeep.Text, out monthsToKeep)))
            {
                result = false;
                MessageBox.Show(Constants.InvalidMonthsToKeep, Constants.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (ComboAccountType.SelectedIndex < 0)
            {
                result = false;
                MessageBox.Show(Constants.MustGiveAnAccountType, Constants.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (result)
            {
                currentAccount.Name = TextAccountName.Text.Trim();
                currentAccount.SetAccountType(accountTypes[ComboAccountType.SelectedIndex]);
                currentAccount.InterestRate = interestRate;
                currentAccount.MonthsToKeep = monthsToKeep;
            }
            return result;
        }

        private void Save()
        {
            if (isModified)
            {
                currentAccount = core.SaveAccountDefinition(currentAccount);
                isModified = false;
            }
        }

        private void SuspendAccount_Click(object sender, RoutedEventArgs e)
        {
            currentAccount.IsActive = !currentAccount.IsActive;
            if (currentAccount.Id != 0)
            {
                core.UpdateAccountActiveStatus(currentAccount);
            }
            UpdateSuspendAccountButton();
        }

        private void UpdateSuspendAccountButton()
        {
            if (currentAccount.IsActive)
            {
                SuspendAccount.Content = Constants.Suspend;
                SuspendAccount.Background = Brushes.Pink;
            }
            else
            {
                SuspendAccount.Content = Constants.Activate;
                SuspendAccount.Background = Brushes.LightGreen;
            }
        }

        private void SaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            if (maintType != MaintType.ReadOnly)
            {
                if (Gather())
                {
                    Save();
                }
            }
            CloseWindow();
        }
    }
}
