using SaltyLogistics.Model;
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
        private CoreModel core;
        private Accounts currentAccount;
        private bool isModified = false;
        private bool readOnly = false;

        public AccountMaintenance()
        {
            InitializeComponent();
            core = CoreModel.GetCoreModel();
        }

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
            core.LoadComboAccountType(ComboAccountType)
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
                Title = NewTitle + currentAccount.AccountName;
                readOnly = IsReadOnly;
            }
            else
            {
                throw new SaltInvalidArgumentType($"Expected {currentAccount.GetType()} received {Params.GetType()}");
            }
        }
    }
}
