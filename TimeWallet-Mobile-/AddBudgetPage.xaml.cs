using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.DTO_s.Json;
using TimeWallet_Mobile_.Data.Translation;

namespace TimeWallet_Mobile_;

public partial class AddBudgetPage : ContentPage
{

    private string _email;
    private ApiService _apiService;
    private Translations _translations;

	public AddBudgetPage()
	{
		InitializeComponent();
        _apiService = new ApiService();
        Shell.SetTabBarIsVisible(this, false);
        ButtonsCalibration();
        GetEmail();
	}

    private async void GetEmail()
    {
        _email = await SecureStorage.GetAsync("UserEmail");
    }

    private async void MainButtonLabel_Clicked(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(txtName.Text))
        {
            await DisplayAlert(_translations.Atention, _translations.ErrorNameLabel, _translations.OkText);
        }
        else if (String.IsNullOrEmpty(txtAmount.Text))
        {
            await DisplayAlert(_translations.Atention, _translations.ErrorAmountLabel, _translations.OkText);
        }
        else
        {
            BudgetAddDTO budgetToAdd = new BudgetAddDTO()
            {
                Id = Guid.NewGuid(),
                Name = txtName.Text,
                Amount = decimal.Parse(txtAmount.Text),
                CreatedAt = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
            };
            string response = await _apiService.AddBudgetAsync(budgetToAdd, _email);
            if (response == $"{txtName.Text} already exists!")
            {
                await DisplayAlert(_translations.Atention, _translations.BudgetAlreadyExists, _translations.OkText);
            }
            else if(response != $"Succesfully added new collection named -{txtName.Text}-!")
            {
                await DisplayAlert(_translations.Atention, _translations.BudgetDecimalPointError, _translations.OkText);
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert(_translations.Atention, _translations.BudgetSuccessfulyAdded + $" -{txtName.Text}- ", _translations.OkText);
                await Navigation.PopAsync();
            }
        }
    }

    private async void ButtonsCalibration()
    {
        string theme = await SecureStorage.GetAsync("Theme");
        if (theme == null)
        {
            await DisplayAlert(_translations.Atention, _translations.Error, _translations.OkText);
        }
        else if (theme == "light")
        {
            Microsoft.Maui.Controls.Application.Current.Resources["Primary"] = Color.FromHex("#e0f2d8");
            Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDark"] = Color.FromHex("#e0f2d8");
            Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkText"] = Color.FromHex("#e0f2d8");
            Microsoft.Maui.Controls.Application.Current.Resources["Secondary"] = Color.FromHex("#e0f2d8");
            Microsoft.Maui.Controls.Application.Current.Resources["SecondaryDarkText"] = Color.FromHex("#e0f2d8");
            Microsoft.Maui.Controls.Application.Current.Resources["Tertiary"] = Color.FromHex("#e0f2d8");
            this.BackgroundColor = Color.FromArgb("#e0f2d8");
        }
        else
        {
            Microsoft.Maui.Controls.Application.Current.Resources["Primary"] = Color.FromHex("#a9d494");
            Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDark"] = Color.FromHex("#a9d494");
            Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkText"] = Color.FromHex("#a9d494");
            Microsoft.Maui.Controls.Application.Current.Resources["Secondary"] = Color.FromHex("#a9d494");
            Microsoft.Maui.Controls.Application.Current.Resources["SecondaryDarkText"] = Color.FromHex("#a9d494");
            Microsoft.Maui.Controls.Application.Current.Resources["Tertiary"] = Color.FromHex("#a9d494");
            this.BackgroundColor = Color.FromArgb("#a9d494");
        }
        string language = await SecureStorage.GetAsync("language");

        if (language == null)
        {
            await DisplayAlert(_translations.Atention, _translations.Error, _translations.OkText);
            await Navigation.PopAsync();
        }
        else if (language == "English")
        {
            _translations = new Translations("en");
        }
        else
        {
            _translations = new Translations("bg");
        }
        SetText();
    }

    private void SetText()
    {
        MainButtonLabel.Text = _translations.BudgetsPageAddBtn;
        MainText.Text = _translations.BudgetsPageMainText;
        NameLabel.Text = _translations.BudgetsPageName;
        txtName.Placeholder = _translations.BudgetsPageName;
        AmountLabel.Text = _translations.BudgetsPageAmount;
        txtAmount.Placeholder = _translations.BudgetsPageAmount;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ButtonsCalibration();
        // Refresh data when the page appears
    }

    private async void goBack_Btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}