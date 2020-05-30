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
        private List<IAccountTypeClient> accountTypes;
        private CoreModel core;
        private Accounts currentAccount;
        private Brush defaultColor;
        private bool isModified = false;
        private bool readOnly = false;
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
            Close();
        }

        public void InitWindow(OpenWindowAction Action, object Params)
        {
            switch (Action)
            {
                case OpenWindowAction.NewItem:
                    InitializeNewAccount();
                    break;
                case OpenWindowAction.ViewItem:
                    InitializeViewAccount(Params);
                    break;
                case OpenWindowAction.EditItem:
                    InitializeEditAccount(Params);
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
            readOnly = false;
        }
        private void InitializeViewAccount(object Params)
        {
            SetAccount(Params, false, Constants.EditAccount);
        }
        private void InitializeEditAccount(object Params)
        {
            SetAccount(Params, true, Constants.ViewAccount);
        }

        private void SetAccount(Object Params, bool IsReadOnly, string NewTitle)
        {
            if (Params is null)
            {
                InitializeNewAccount();
            }
            else if (Params is Accounts)
            {
                currentAccount = Params as Accounts;
                Title = NewTitle + currentAccount.Name;
                readOnly = IsReadOnly;
            }
            else
            {
                throw new SaltInvalidArgumentType($"Expected {currentAccount.GetType()} received {Params.GetType()}");
            }
        }

        private void Scatter()
        {
            if (readOnly)
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

        private void _TextChanged(object sender, TextChangedEventArgs e)
        {
            AccountHasBeenModified();
            if (!isModified)
            {
                isModified = true;
                AccountChanged.Content = Constants.Changed;
            }
        }

        private void _SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            currentAccount = core.SaveAccountDefinition(currentAccount);
            isModified = false;
        }
    }
}
