using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.DTO_s.Json;

namespace TimeWallet_Mobile_;

public partial class AddBudgetPage : ContentPage
{

    private string _email;
    private ApiService _apiService;

	public AddBudgetPage()
	{
		InitializeComponent();
        _apiService = new ApiService();
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
}