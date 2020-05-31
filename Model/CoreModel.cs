using SaltyLogistics.View;
using SaltyLogistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SaltyLogistics.Preferences;
using System.Windows.Interop;

/**
 * <summary>
 * Central core of this application.
 * This is a singleton object that is created at the start of the application and remains in memory until shutdown
 * All new windows are initiated through this core module. It always knows what windows are open. A specific handling
 * view model is created for each window that is opened. That view model will initialize and control the window.
 * All communication with the stored data goes through this module.
 * All cross communication between windows goes through this module.
 * </summary>
 */
namespace SaltyLogistics.Model 
{
    public sealed class CoreModel
    {
        /**
         * Singleton support
         */
        private static CoreModel _coreModel = null;
        private static readonly object padlock = new object();

        /**
         * Connect to the database support
         */
        private readonly ISaltMine saltMine;
        private readonly Preferences.IPreferences preferences;
        private readonly List<ISaltWindowBase> openWindows;
        private bool shuttingDown;
        private CoreModel()
        {
            shuttingDown = false;
            SaltMine mine = new SaltMine();
            saltMine = mine;
            preferences = new Preferences.Preferences(mine);
            openWindows = new List<ISaltWindowBase>();
        }

        private void Init()
        {
            
        }
        
        public static CoreModel GetCoreModel()
        {
            lock (padlock) // Make thread safe
            {
                if (_coreModel == null)
                {
                    _coreModel = new CoreModel();
                    _coreModel.Init();

                }
            }
            return _coreModel;
        }

        public bool GetShowingAllAccounts()
        {
            return preferences.GetConfigBool(Constants.ProgSetting, Constants.ShowAllAccounts);
        }

        public List<IAccountTypeClient> LoadComboAccountType(ComboBox ComboAccountTypes)
        {
            List<IAccountTypeClient> accountTypes = saltMine.GetAccountTypesActive();
            ComboAccountTypes.Items.Clear();
            foreach (IAccountTypeClient accountType in accountTypes)
            {
                ComboAccountTypes.Items.Add(accountType.Name);
            }
            return accountTypes;

        }

        public void UpdateAccountActiveStatus(Accounts currentAccount)
        {
            saltMine.UpdateAccountActiveStatus(currentAccount.Id, currentAccount.IsActive);
        }

        public void UpdateShowingAllAccounts(bool isShowing)
        {
            preferences.SetConfigBool(Constants.ProgSetting, Constants.ShowAllAccounts, isShowing);
        }

        /** 
         * Shut down any open windows and threads that may be running.
         * Do not return until they are shutdown.
         */
        public void ShutDown()
        {
            shuttingDown = true;
            foreach (ISaltWindowBase saltWindow in openWindows)
            {
                saltWindow.CloseWindow();
            }
        }

        public void ShowAccountMaintenance(OpenWindowAction Action, object Info = null)
        {
            AccountMaintenance accountMaint = new AccountMaintenance();
            accountMaint.InitWindow(Action, Info);
            openWindows.Add(accountMaint);
            accountMaint.Show();
        }

        public Accounts SaveAccountDefinition(Accounts Account) => saltMine.SaveAccountDefinition(Account);

        public void WindowIsClosing(ISaltWindowBase Win)
        {
            if (!shuttingDown)
            {
                for (int winIx = 0; winIx < openWindows.Count; winIx++)
                {
                    if (Win == openWindows[winIx])
                    {
                        openWindows.RemoveAt(winIx);
                        break;
                    }
                }
            }
        }
    }


}
