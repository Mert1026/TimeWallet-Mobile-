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
        SetLanguage();
        Shell.SetNavBarIsVisible(this, false);
        Shell.SetTabBarIsVisible(this, false);
        SetText();
        // CheckIfThereIsAlreadyLogedAccount();
    }

    private void SetText()
    {
        SetLanguage();  
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
                Username = user.user.Name,
                //TO DO
                Remember = true

            };
            string message = await _apiService.LoginUser(login);
            if (message == "Login Successful.")
            {
                await SecureStorage.SetAsync("UserEmail", txtEmail.Text);
                await SecureStorage.SetAsync("UserName", user.user.Name);

                //Preferences
                await SecureStorage.SetAsync("theme", user.user.Theme);
                await SecureStorage.SetAsync("language", user.user.Language);


                await Shell.Current.GoToAsync("//userMainTest");

            }

        }
    

    }

    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        // Navigate to RegisterPage (make sure to create a RegisterPage.xaml)
        await Navigation.PushAsync(new RegisterPage());
        //await Shell.Current.GoToAsync("//userMainTest");
    }

    private async void SetLanguage()
    {
        string language = await SecureStorage.GetAsync("language");
        if(language == "bg")
        {
            _language = "bg";
        }
        else
        {
            _language = "en";
        }
        
        _translations = new Translations(_language);
        SetText();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetLanguage();
        SetText();  // Update text when the page appears
    }

}