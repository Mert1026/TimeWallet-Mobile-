using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.Translation;

namespace TimeWallet_Mobile_;

public partial class SettingsPage : ContentPage
{
    private Translations _translations;
    private ApiService _apiService;
    private string _selectedLanguage;
    private string _selectedTheme;
	public SettingsPage()
	{
		InitializeComponent();
        _apiService = new ApiService();
        ButtonsCalibration();
        PickerFill();
        //SetText();
	}

    private void PickerFill()
    {
        LanguagePicker.Items.Add("bg");
        LanguagePicker.Items.Add("en");
    }

    private async void ButtonsCalibration()
    {
        string theme = await SecureStorage.GetAsync("Theme");
        if(theme == null)
        {
            await DisplayAlert("Atention", "Error", "Ok");
        }
        else if(theme == "light")
        {
            _selectedTheme = "light";
            ThemeBtn.BackgroundColor = Color.FromArgb("#e0f2d8");
            this.BackgroundColor = Color.FromArgb("#e0f2d8");
            ThemeBtn.Source = "sun.svg";
        }
        else
        {
            _selectedTheme = "night";
            ThemeBtn.BackgroundColor = Color.FromArgb("#a9d494");
            this.BackgroundColor = Color.FromArgb("#a9d494");
            ThemeBtn.Source = "sunandcloud.svg";
        }

        string language = await SecureStorage.GetAsync("language");
        _selectedLanguage = language;

        if(language == null)
        {
            await DisplayAlert("Atention", "Error occured! Try again later.", "Ok");
            await Navigation.PopAsync();
        }
        else if(language ==  "en")
        {
            _translations = new Translations("en");
            LanguagePicker.SelectedIndex = 1;
        }
        else
        {
            _translations = new Translations("bg");
            LanguagePicker.SelectedIndex = 0;
        }

        UserNameEntry.Text = await SecureStorage.GetAsync("UserName");
        EmailEntry.Text = await SecureStorage.GetAsync("UserEmail");
        SetText();
    }

    private async void ThemeBtn_Clicked(object sender, EventArgs e)
    {
        string result = await SecureStorage.GetAsync("Theme");
        if(result == "light")
        {
            await SecureStorage.SetAsync("Theme", "night");
        }
        else
        {
            await SecureStorage.SetAsync("Theme", "light");
        }
        ButtonsCalibration();
    }

    private async void LanuagePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Microsoft.Maui.Controls.Picker)sender;
        string selectedLanguage = (string)picker.SelectedItem;
        _selectedLanguage = selectedLanguage;
        await SecureStorage.SetAsync("language", selectedLanguage);
        _translations = new Translations(selectedLanguage);
        ButtonsCalibration();
        SetText();
    }

    private void SetText()
    {
        MainText.Text = _translations.SettingsMainText;
        LangualeLabel.Text = _translations.LanguageLabelText;
        ThemeLabel.Text = _translations.ThemeLabelText;
        MainText_2.Text = _translations.UserSettingsMainText;
        NameLabel.Text = _translations.UserNameLabelText;
        EmailLabel.Text = _translations.UserEmailLabelText;
        SaveBtn.Text = _translations.SettingsPageSaveBtnText;
    }

    private async void SaveBtn_Clicked(object sender, EventArgs e)
    {
        string email = await SecureStorage.GetAsync("UserEmail");
        string result = await _apiService.UpdateUserAsync(email, UserNameEntry.Text, EmailEntry.Text, _selectedLanguage, _selectedTheme);
        if(String.IsNullOrEmpty(EmailEntry.Text) || String.IsNullOrEmpty(UserNameEntry.Text))
        {
            await DisplayAlert("Atention", "Error occured! All fields are required..", "Ok");
        }
        else if(result == "User successfully updated.")
        {
            await SecureStorage.SetAsync("UserEmail", EmailEntry.Text);
            await SecureStorage.SetAsync("UserName", UserNameEntry.Text);
            await DisplayAlert("Success", "Successfuly updated User.", "Ok");
        }
        else
        {
            await DisplayAlert("Atention", "Error occured! Try again later.", "Ok");
        }
    }
}