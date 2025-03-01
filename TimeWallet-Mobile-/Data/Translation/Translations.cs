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
        

        //Expenses/ElementsPage

        //MainPage
        public string MainPageExpensesText { get; set; }
        public string mainPageBudgetButtonText { get; set; }
        public string MainPageElementButtonText {  get; set; }  
        public string MainPageCameraButtonText {  get; set; }

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
            }
        }

    }
}
