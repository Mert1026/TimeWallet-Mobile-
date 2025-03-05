
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TimeWallet_Mobile_.Data.Translation
{
    public class Translations
    {
        //Register page
        public string RegisterPageMainText { get; set; }
        //Другите са зададени по надолу.(Преизползване)


        //StartUpPage
        public string StartUpMainText { get; set; }
        public string StartUpVisitText { get; set; }

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
        public string BudgetsCurrentBalance {  get; set; }
        

        //AddExpensesPage
        public string ExpensePageMainText { get; set; }
        public string ExpensePageName {  get; set; }
        public string ExpensePageAmount { get; set; }
        public string ExpensePageBudgetSelect {  get; set; }
        public string ExpensePageAddBtn { get; set; }


        //AddBudgetsPage
        public string BudgetsPageMainText { get; set; }
        public string BudgetsPageName { get; set; }
        public string BudgetsPageAmount { get; set; }
        public string BudgetsPageAddBtn { get; set; }
        //Warns:
        public string Atention {  get; set; }
        public string Success { get; set; }
        public string OkText { get; set; }
        public string BudgetAlreadyExists { get; set; }
        public string BudgetSuccessfulyAdded { get; set; }
        public string BudgetDecimalPointError { get; set; }
        public string Error { get; set; }
        public string ErrorNameLabel { get; set; }
        public string ErrorAmountLabel {  get; set; }
        public string DontHaveAnyBudgets { get; set; }
        public string DontHaveSelectedBudget { get; set; }
        public string CameraPermissionError { get; set; }
        public string DontHaveReceiptExisting { get; set; }
        public string ErrorBarcode { get; set; }
        public string SuccessAddedExpenses { get; set; }
        public string DontOwnBudget {  get; set; }
        public string SuccessDeletedBudged { get; set; }
        public string DeleteLabel { get; set; }


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
        public string SettingsPageLogOutBtnText { get; set; }

        //Camera

        public string SelectionPageMainText { get; set; }
        public string SelectionPageInfoLabel {  get; set; }
        public string SelectionPageInfoText { get; set; }

        public Translations(string language) 
        {
            RegisterPageMainText = "Register";

            StartUpMainText = "Welcome to, TimeWallet!";
            StartUpVisitText = "Don't forget to visit our website, too!";

            DeleteLabel = "DELETE";

            LoginPageMainText = "Login";
            LoginPageEmailText = "Email";
            LoginPageFooterText = "Don't have an account? Click here.";
            LoginPagePasswordText = "Password";
            LoginPageRememberMeText = "Remember me?";

            BudgetsSecondarytext = "Budgeted";
            BudgetsThirdlyText_One = "Spent";
            BudgetsThirdlyText_Two = "Remaining";
            BudgetsButtonText = "View Details";
            BudgetsCurrentBalance = "Current Balance";

            SettingsMainText = "Settings";
            UserSettingsMainText = "Profile Settings";
            LanguageLabelText = "Language";
            ThemeLabelText = "Theme";
            UserNameLabelText = "User Name";
            UserEmailLabelText = "Email";
            SettingsPageSaveBtnText = "Save";
            SettingsPageLogOutBtnText = "Log Out";

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

            Atention = "Atention";
            Success = "Success";
            OkText = "Ok";
            Error = "Error";
            ErrorNameLabel = "The Name label is required! Consider filling it.";
            ErrorAmountLabel = "The Amount label is required! Consider filling it.";
            BudgetAlreadyExists = "Budget with this name already exists! Consider changing the name.";
            BudgetDecimalPointError = "Something went wrong pleasy try again later.(Consider changing the decimal point)";
            BudgetSuccessfulyAdded = "Succesfully added new collection named";
            DontHaveAnyBudgets = "You don't have any budgets in order to add element. Consider making one.";
            DontHaveSelectedBudget = "You should pick where to add this element! Consider picking it.";
            CameraPermissionError = "Camera permission is required to use this feature.";
            DontHaveReceiptExisting = "There is no receipt in existence with the following parameters!";
            ErrorBarcode = "Invalid QR-code format.";
            SuccessAddedExpenses = "Expenses have been added to the selected budgets.";
            DontOwnBudget = "You don't own the provided budget!";
            SuccessDeletedBudged = "Budged deleted successfuly";

            SelectionPageMainText = "Select Budgets:";
            SelectionPageInfoLabel = "Info";
            SelectionPageInfoText = "Here in this page for every item in your receipt you should select a corresponding budget about where it to be added.";


            //        if (response == $"{txtName.Text} already exists!")
            //            {
            //                await DisplayAlert("Atention", "Budget with this name already exists! Consider changing the name.", "Ok");
            //    }
            //            else if(response != $"Succesfully added new collection named -{txtName.Text}-!")
            //            {
            //                await DisplayAlert("Atention", "Something went wrong pleasy try again later.(Consider changing the decimal point)", "Ok");
            //    await Navigation.PopAsync();
            //}
            //            else
            //{
            //    await DisplayAlert("Success", $"Succesfully added new collection named -{txtName.Text}-!", "Ok");
            //    await Navigation.PopAsync();
            //}

            if (language == "bg")
            {
                RegisterPageMainText = "Регистрация";

                StartUpMainText = "Добре дошли в, TimeWallet!";
                StartUpVisitText = "Не забравяйте да посетите и нашия уеб-сайт!";

                DeleteLabel = "ИЗТРИВАНЕ";

                LoginPageMainText = "Вход";
                LoginPageEmailText = "Имейл";
                LoginPageFooterText = "Нямате акаунт? Кликнете тук.";
                LoginPagePasswordText = "Парола";
                LoginPageRememberMeText = "Запомни ме?";

                BudgetsSecondarytext = "Предвидени";
                BudgetsThirdlyText_One = "Изхарчено";
                BudgetsThirdlyText_Two = "Оставащо";
                BudgetsButtonText = "Детайли";
                BudgetsCurrentBalance = "Настоящ Баланс";


                SettingsMainText = "Настройки";
                UserSettingsMainText = "Профил";
                LanguageLabelText = "Език";
                ThemeLabelText = "Тема";
                UserNameLabelText = "Име";
                UserEmailLabelText = "Имейл";
                SettingsPageSaveBtnText = "Запамети";
                SettingsPageLogOutBtnText = "Изход";

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

                Atention = "Внимание";
                Success = "Успех";
                OkText = "Добра";
                Error = "Грешка";
                ErrorNameLabel = "Полето за име е задължително! Обмислете да го запълните.";
                ErrorAmountLabel = "Полето за стойност е задължително! Обмислете да го запълните.";
                BudgetAlreadyExists = "Бюджет с това име вече съществува! Обмислете да го промените.";
                BudgetDecimalPointError = "Грешка.(Обмислете заменянето на десетичната точка)";
                BudgetSuccessfulyAdded = "Успешно добавихте бюджет на име";
                DontHaveAnyBudgets = "Нямате добавени бюджети за да добавите разход някъде. Обмислете направата на бюджет.";
                DontHaveSelectedBudget = "Трябва да изберете къде да добавите този разход! Обмислете избирането на бюджет.";
                CameraPermissionError = "Нужен е достъп до камерата за а използвате тази функция.";
                DontHaveReceiptExisting = "Несъществува касов бон със следните параметри!";
                ErrorBarcode = "Невалиден формат на QR-кода";
                SuccessAddedExpenses = "Разходите бяха добавени в избраните бюджети.";
                DontOwnBudget = "Не притежавате този бюджет!";
                SuccessDeletedBudged = "Бюджета е изтрит успешно";

                SelectionPageMainText = "Изберете бюджети:";
                SelectionPageInfoLabel = "Информация";
                SelectionPageInfoText = "Тук, в тази страница, за всеки артикул във вашия касов бонр вие трябва да изберете съответния бюджет за това къде да бъде добавен.";

            }
        }

    }
}
