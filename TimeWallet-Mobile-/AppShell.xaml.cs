namespace TimeWallet_Mobile_
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
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
            //string email = await SecureStorage.GetAsync("UserEmail");

            //if (string.IsNullOrEmpty(email))
            //{
            //    // Navigate to LoginPage if no email is found
            //    await GoToAsync(nameof(LoginPage));
            //}
            //else
            //{
            //    // Navigate to Home (default tab)
            //    await GoToAsync(nameof(LoginPage));
            //}
        }
    }
}
