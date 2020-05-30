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
        private ISaltMine saltMine;
        private Preferences.IPreferences preferences;
        private List<ISaltWindowBase> openWindows;
        private CoreModel()
        {
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
            foreach (ISaltWindowBase saltWindow in openWindows)
            {
                saltWindow.CloseWindow();
            }
        }

        public void ShowAccountMaintenance(OpenWindowAction Action, string Id = "")
        {
            ISaltWindowBase AccountMaint = new AccountMaintenance();
        }

        public Accounts SaveAccountDefinition(Accounts Account)
        {
            return ISaltMine.SaveAccountDefinition(Account);
        }
    }


}
