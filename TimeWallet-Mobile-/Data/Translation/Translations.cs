
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeWallet_Mobile_.Data.Translation
{
    public class Translations
    {
        //LoginPage
        public string LoginPageMainText { get; set; }
        public string LoginPageEmailText { get; set; }
        public string LoginPagePasswordText { get; set; }
        public string LoginPageRememberMeText { get; set; }
        public string LoginPageFooterText { get; set; }
        
        
        //BudgetsPage
        public string BudgetsSecondarytext { get; set; }
        public string BudgetsThirdlyText_One { get; set; }
        public string BudgetsThirdlyText_Two { get; set; }
        public string BudgetsButtonText { get; set; }
        

        //ExpensesPage
        public string ExpensePageMainText { get; set; }
        public string ExpensePageName {  get; set; }
        public string ExpensePageAmount { get; set; }
        public string ExpensePageBudgetSelect {  get; set; }
        public string ExpensePageAddBtn { get; set; }


        //BudgetsPage
        public string BudgetsPageMainText { get; set; }
        public string BudgetsPageName { get; set; }
        public string BudgetsPageAmount { get; set; }
        public string BudgetsPageAddBtn { get; set; }


        //MainPage
        public string MainPageMainText { get; set; }
        public string MainPageBudgetButtonText { get; set; }
        public string MainPageElementButtonText {  get; set; }  
        public string MainPageCameraButtonText {  get; set; }

        //SettingsPage

        public string SettingsMainText { get; set; }
        public string UserSettingsMainText { get; set; }
        public string LanguageLabelText { get; set; }
        public string ThemeLabelText { get; set; }
        public string UserNameLabelText { get; set; }
        public string UserEmailLabelText {  get; set; }
        public string SettingsPageSaveBtnText { get; set; }

        public Translations(string language) 
        {

            LoginPageMainText = "Login";
            LoginPageEmailText = "Email";
            LoginPageFooterText = "Don't have an account? Click here.";
            LoginPagePasswordText = "Password";
            LoginPageRememberMeText = "Remember me?";

            BudgetsSecondarytext = "Budgeted";
            BudgetsThirdlyText_One = "Spent";
            BudgetsThirdlyText_Two = "Remaining";
            BudgetsButtonText = "View Details";

            SettingsMainText = "Settings";
            UserSettingsMainText = "Profile Settings";
            LanguageLabelText = "Language";
            ThemeLabelText = "Theme";
            UserNameLabelText = "User Name";
            UserEmailLabelText = "Email";
            SettingsPageSaveBtnText = "Save";

            MainPageMainText = "Last Expenses";
            MainPageBudgetButtonText = "Add Budget";
            MainPageElementButtonText = "Add Expense";
            MainPageCameraButtonText = "Camera";

            ExpensePageAddBtn = "Add Expense";
            ExpensePageAmount = "Amount";
            ExpensePageBudgetSelect = "Select Budget:";
            ExpensePageMainText = "Add Expense";
            ExpensePageName = "Name";

            BudgetsPageAddBtn = "Add Budget";
            BudgetsPageAmount = "Amount";
            BudgetsPageMainText = "Add Budget";
            BudgetsPageName = "Name";

            if (language == "bg")
            {
                LoginPageMainText = "Вход";
                LoginPageEmailText = "Имейл";
                LoginPageFooterText = "Нямате акаунт? Кликнете тук.";
                LoginPagePasswordText = "Парола";
                LoginPageRememberMeText = "Запомни ме?";

                BudgetsSecondarytext = "Предвидено";
                BudgetsThirdlyText_One = "Изхарчено";
                BudgetsThirdlyText_Two = "Оставащо";
                BudgetsButtonText = "Детайли";

                SettingsMainText = "Настройки";
                UserSettingsMainText = "Профил";
                LanguageLabelText = "Език";
                ThemeLabelText = "Тема";
                UserNameLabelText = "Име";
                UserEmailLabelText = "Имейл";
                SettingsPageSaveBtnText = "Запамети";

                MainPageMainText = "Скорошни разходи";
                MainPageBudgetButtonText = "Добави Бюджет";
                MainPageElementButtonText = "Добави Разход";
                MainPageCameraButtonText = "Камера";

                ExpensePageAddBtn = "Добави";
                ExpensePageAmount = "Стойност";
                ExpensePageBudgetSelect = "Избери бюджет:";
                ExpensePageMainText = "Добави разход";
                ExpensePageName = "Име";

                BudgetsPageAddBtn = "Добави Бюджет";
                BudgetsPageAmount = "Стойност";
                BudgetsPageMainText = "Добави Бюджет";
                BudgetsPageName = "Име";
            }
        }

    }
}
