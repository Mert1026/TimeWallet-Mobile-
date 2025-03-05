using Newtonsoft.Json;
using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.Models;
using TimeWallet_Mobile_.Data.DTO_s;
using TimeWallet_Mobile_.Data.DTO_s.Json;
using TimeWallet_Mobile_.Data.Translation;

namespace TimeWallet_Mobile_;

public partial class AddElementPage : ContentPage
{
	public List<string> BudgetsOptions = new List<string>();
    public List<Budgets> _budgets = new List<Budgets>();
    private ApiService _apiService;
    private string _email;
    private Translations _translations;
	public AddElementPage()
	{
        _apiService = new ApiService();
        InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);
        ButtonsCalibration();
        BindingContext = this;
        GetUserEmail();
        AddBudgetsToPicker();
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        string selectedCategory = (string)picker.SelectedItem;
    }

    private async void GetUserEmail()
    {
        _email = await SecureStorage.GetAsync("UserEmail");
    }

    private async void AddBudgetsToPicker()
    {
        //If there are no budgets
        _email = await SecureStorage.GetAsync("UserEmail");
        var userBudgetResponse = await _apiService.GetInformationAboutUser(_email);
        var budgets = JsonConvert.DeserializeObject<List<Budgets>>(userBudgetResponse.budgetJson);
        if (budgets == null)
        {
            await DisplayAlert(_translations.Atention, _translations.Error, _translations.OkText);
            await Navigation.PopAsync();
        }
   else if (!(budgets.Count > 0))
        {
            await DisplayAlert(_translations.Atention, _translations.DontHaveAnyBudgets, _translations.OkText);
            await Navigation.PopAsync();
        }
        _budgets = budgets;
        foreach(string el in budgets.Select(x => x.Name))
        {
            BudgetsPicker.Items.Add(el);
        }
        OnAppearing();
    }

    //Add elemenet(Името е такова поради проблеми породили се от променянето му)
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
        else if (BudgetsPicker.SelectedIndex == -1)
        {
            await DisplayAlert(_translations.Atention, _translations.DontHaveSelectedBudget, _translations.OkText);
        }
        else
        {
            string selectedValue = BudgetsPicker.SelectedItem.ToString();
            Guid budgetId = _budgets.FirstOrDefault(b => b.Name == selectedValue).Id;
            ElementDTO elementToAdd = new ElementDTO()
            {
                id = Guid.NewGuid(),
                name = txtName.Text,
                amount = decimal.Parse(txtAmount.Text),
                budgetId = budgetId,
                createdAt = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
            };

            string result = await _apiService.AddElementAsync(elementToAdd, _email);
            await Navigation.PopAsync();
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
        MainText.Text = _translations.ExpensePageMainText;
        NameLabel.Text = _translations.ExpensePageName;
        txtName.Placeholder = _translations.ExpensePageName;
        AmountLabel.Text = _translations.ExpensePageAmount;
        txtAmount.Placeholder = _translations.ExpensePageAmount;
        BudgetLabel.Text = _translations.ExpensePageBudgetSelect;
        MainButton.Text = _translations.ExpensePageAddBtn;

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