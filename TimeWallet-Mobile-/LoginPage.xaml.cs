using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.DTO_s.Json;

namespace TimeWallet_Mobile_;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    //private UserMethods userMethods = new UserMethods();

    public LoginPage()
    {
        InitializeComponent();

        Shell.SetNavBarIsVisible(this, false);
        // CheckIfThereIsAlreadyLogedAccount();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        UserDTO user = await _apiService.GetUserAsync(txtEmail.Text);
        if (user.User.Name == null)
        {

        }
        else
        {
            LoginModelDTO login = new LoginModelDTO()
            {
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Username = user.User.Name,
                //TO DO
                Remember = true

            };
            string message = await _apiService.LoginUser(login);
            if (message == "Login Successful.")
            {
                await SecureStorage.SetAsync("UserEmail", txtEmail.Text);
                await SecureStorage.SetAsync("UserName", user.User.Name);
                

                await Navigation.PushAsync(new NewPage1());
                


                //Microsoft.Maui.Controls.Application.Current.MainPage = new Microsoft.Maui.Controls.NavigationPage(new UserMainPage());
                //Microsoft.Maui.Controls.Application.Current.MainPage = new AppShell();
            }

        }
    

    }

    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        // Navigate to RegisterPage (make sure to create a RegisterPage.xaml)
        //await Navigation.PushAsync(new RegisterPage());
        await Navigation.PushAsync(new NewPage1());
    }
}