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
            await DisplayAlert("Atention", "The Name label is required! Consider filling it.", "Ok");
        }
        else if (String.IsNullOrEmpty(txtAmount.Text))
        {
            await DisplayAlert("Atention", "The Amount label is required! Consider filling it.", "Ok");
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
                await DisplayAlert("Atention", "Budget with this name already exists! Consider changing the name.", "Ok");
            }
            else if(response != $"Succesfully added new collection named -{txtName.Text}-!")
            {
                await DisplayAlert("Atention", "Something went wrong pleasy try again later.(Consider changing the decimal point)", "Ok");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Success", $"Succesfully added new collection named -{txtName.Text}-!", "Ok");
                await Navigation.PopAsync();
            }
        }
    }

    private async void ButtonsCalibration()
    {
        string theme = await SecureStorage.GetAsync("Theme");
        if (theme == null)
        {
            await DisplayAlert("Atention", "Error", "Ok");
        }
        else if (theme == "light")
        {
            this.BackgroundColor = Color.FromArgb("#e0f2d8");
        }
        else
        {
            this.BackgroundColor = Color.FromArgb("#a9d494");
        }

        string language = await SecureStorage.GetAsync("language");

        if (language == null)
        {
            await DisplayAlert("Atention", "Error occured! Try again later.", "Ok");
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

    private void SetText()
    {
        MainButtonLabel.Text = _translations.BudgetsPageAddBtn;
        MainText.Text = _translations.MainPageMainText;
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