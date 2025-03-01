using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.DTO_s.Json;
using TimeWallet_Mobile_.Data.Translation;

namespace TimeWallet_Mobile_;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();
    private Translations _translations;
    private string _language = "en";
    //private UserMethods userMethods = new UserMethods();

    public LoginPage()
    {
        InitializeComponent();
        _translations = new Translations(_language);
        SetText();
        Shell.SetNavBarIsVisible(this, false);
        // CheckIfThereIsAlreadyLogedAccount();
    }

    private void SetText()
    {
        MainTextLabel.Text = _translations.LoginPageMainText;
        EmailLabel.Text = _translations.LoginPageEmailText;
        PasswordLabel.Text = _translations.LoginPagePasswordText;
        RememberMeLabel.Text = _translations.LoginPageRememberMeText;
        MainButtonLabel.Text = _translations.LoginPageMainText;
        RegisterNavigationLabel.Text = _translations.LoginPageFooterText;
        //InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        UserDTO user = await _apiService.GetUserAsync(txtEmail.Text);
        if (user == null)
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
                
            }

        }
    

    }

    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        // Navigate to RegisterPage (make sure to create a RegisterPage.xaml)
        //await Navigation.PushAsync(new RegisterPage());
        await Navigation.PushAsync(new RegisterPage());
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if(_language == "bg")
        {
            _language = "en";
        }
        else
        {
            _language = "bg";
        }
        
        _translations = new Translations(_language);
        SetText();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetText();  // Update text when the page appears
    }

}