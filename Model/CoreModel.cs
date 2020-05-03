﻿using SaltyLogistics.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

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

        public ISaltPiles SaltPiles { get; private set; }
        
        private CoreModel()
        {
            saltMine = new SaltMine();
        }

        private void Init()
        {
            SaltPiles = new SaltPiles();
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

        public bool getConfigBool(string section, string preference, bool defaultValue = false)
        {
            string value = saltMine.GetConfig(section, preference);
            return (value == Constants.True);
        }

        public void setConfigBool(string section, string preference, bool newValue)
        {
            string valueToStore = (newValue) ? Constants.True : Constants.False;
            saltMine.SetConfig(section, preference, valueToStore);
        }

    }


}
