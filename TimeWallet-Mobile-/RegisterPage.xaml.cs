using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.Models;

namespace TimeWallet_Mobile_;

public partial class RegisterPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public RegisterPage()
    {
        InitializeComponent();
    }

    // private ApplicationDbContext context = new ApplicationDbContext();

    private async void RegisterBtn_Clicked(object sender, EventArgs e)
    {
        //string UserName = txtUserName.Text;
        //string Password = txtPassword.Text;

        //      if (context.Users.FirstOrDefault(u => u.UserName == UserName) == null)
        //      {
        //          UserNameDisplayed.Text = "NO";
        //      }
        //      else
        //      {
        //          UserNameDisplayed.Text = "YES";
        //          await Task.Delay(1000);
        //          await Navigation.PushAsync(new SuccessPage());
        //      }

        //PasswordDisplayed.Text = Password;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        User user = new User()
        {
            Name = txtName.Text,
            Email = txtEmail.Text,
            UserName = txtName.Text,
            PasswordHash = txtPassword.Text,
        };

        //string message = await _apiService.RegisterUser(user);
        await Console.Out.WriteLineAsync();

    }

    private async void OnHaveAnAccount_Tapped(object sender, TappedEventArgs e)
    {
        //await Navigation.PushAsync(new Login());
    }
}