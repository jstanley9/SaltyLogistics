using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics
{
    class Constants
    {
        public static readonly string AccountIdType = "AccountIdType";
        public static readonly string Activate = "Activate";
        public static readonly string AddAccount = "Add " + ViewAccount;
        public static readonly string AnAccountMustHaveAName = "The account must have a name!";
        public static readonly string AppName = "Salt";
        public static readonly string At_AccountTypeId = "@AccountTypeId";
        public static readonly string At_Id = "@Id";
        public static readonly string At_InterestRate = "@InterestRate";
        public static readonly string At_IsActive = "@IsActive";
        public static readonly string At_Name = "@Name";
        public static readonly string At_MonthsToKeep = "@MonthsToKeep";
        public static readonly string At_Preference = "@preference";
        public static readonly string At_Section = "@section";
        public static readonly string At_Value = "@value";
        public static readonly string Changed = "*** Changed ***";
        public static readonly string EditAccount = "Edit " + ViewAccount;
        public static readonly string False = "False";
        public static readonly string GetAccounttypesActive = "GetAccountTypesActive";
        public static readonly string InvalidInterestRate = "Invalid interest rate";
        public static readonly string InvalidMonthsToKeep = "Invalid months to keep transactions";
        public static readonly string IsActive = "IsActive";
        public static readonly string IsAsset = "IsAsset";
        public static readonly string IsInterestComputed = "IsInterestComputed";
        public static readonly string MustGiveAnAccountType = "Must select an account type";
        public static readonly string Name = "Name";
        public static readonly string NameId = "NameId";
        public static readonly string ProgSetting = "ProgSetting";
        public static readonly string SaltPileSQLProvider = "SaltPileSQLProvider";
        public static readonly string Save = "Save";
        public static readonly string SaveAndClose = "Save/Close";
        public static readonly string ShowAllAccounts = "ShowAllAccounts";
        public static readonly string SP_GetConfig = "dbo.GetConfig";
        public static readonly string SP_InsertAccounts = "InsertAccounts";
        public static readonly string SP_UpdateAccounts = "UpdateAccounts";
        public static readonly string SP_UpdateConfig = "dbo.UpdateConfig";
        public static readonly string SP_UpdateAccountActiveStatus = "UpdateAccountActiveStatus";
        public static readonly string StatusActive = "*** Active ***";
        public static readonly string StatusSuspended = "*** Suspended ***";
        public static readonly string Suspend = "Suspend";
        public static readonly string True = "True";
        public static readonly string ViewAccount = "Account ";

        public static readonly int Name_Length = 30;
    }
}
