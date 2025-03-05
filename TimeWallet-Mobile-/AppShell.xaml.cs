using Microsoft.Maui.Controls;

namespace TimeWallet_Mobile_
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            //CurrentItem = Items.FirstOrDefault(item => item.Route == "userMainTest");

            InitializeComponent();

            
            SetNavBarIsVisible(this, false);
            Routing.RegisterRoute(nameof(NewPage1), typeof(NewPage1));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            NavigationPage.SetHasBackButton(new NewPage1(), false);

            // Navigate to Profile page on startup
            SetStartupPage();

        }

        private async void SetStartupPage()
        {

            string email = "";
            //string email = "memo@gmail.com";

            if (string.IsNullOrEmpty(email))
            {
                // Navigate to LoginPage if no email is found
                //await Navigation.PushAsync(new StartUpPage());
            }
            else
            {
                // Navigate to Home (default tab)
                 await GoToAsync(nameof(UserMainPage_TEST));
                //CurrentItem = Items.FirstOrDefault(item => item.Route == "userMainTest");
                //await Shell.Current.GoToAsync(new UserMainPage_TEST());
            }
        }
    }
}
