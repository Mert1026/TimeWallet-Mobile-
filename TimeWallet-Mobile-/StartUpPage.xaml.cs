using TimeWallet_Mobile_.Data.Translation;

namespace TimeWallet_Mobile_;

public partial class StartUpPage : ContentPage
{

    private Translations _translations;

	public StartUpPage()
	{
		InitializeComponent();
        ButtonsCalibration();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }

    private async void LanguagePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        ButtonsCalibration();
        if (LanguagePicker.SelectedIndex != -1)
        {
            string selectedItem = LanguagePicker.Items[LanguagePicker.SelectedIndex];
            if(selectedItem != null)
            {
                if(selectedItem == "English")
                {
                    _translations = new Translations("en");
                    await SecureStorage.SetAsync("language", "English");
                    RegisterBtn.WidthRequest = 130;
                }
                else
                {
                    _translations = new Translations("bg");
                    await SecureStorage.SetAsync("language", "bg");
                    RegisterBtn.WidthRequest = 160;
                }
                SetText();
            }
        }
    }

    private async void SetText()
    {
        ButtonsCalibration();
        MainTextLabel.Text = _translations.StartUpMainText;
        LoginBtn.Text = _translations.LoginPageMainText;
        RegisterBtn.Text = _translations.RegisterPageMainText;
        VisitLabel.Text = _translations.StartUpVisitText;
    }

    private async void ButtonsCalibration()
    {
        string language = await SecureStorage.GetAsync("language");

        if (language == null)
        {
            await DisplayAlert(_translations.Atention, _translations.Error, _translations.OkText);
            await Navigation.PopAsync();
        }
        else if (language == "en")
        {
            _translations = new Translations("en");
        }
        else
        {
            _translations = new Translations("bg");
        }
        SetText();
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        string url = "https://timewallet.azurewebsites.net/welcome";
        await Launcher.OpenAsync(url);
    }
}