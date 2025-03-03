﻿using Newtonsoft.Json;
using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.Models;
using TimeWallet_Mobile_.Data.DTO_s;
using TimeWallet_Mobile_.Data.DTO_s.Json;

namespace TimeWallet_Mobile_;

public partial class AddElementPage : ContentPage
{
	public List<string> BudgetsOptions = new List<string>();
    public List<Budgets> _budgets = new List<Budgets>();
    private ApiService _apiService;
    private string _email;
	public AddElementPage()
	{
        _apiService = new ApiService();
        InitializeComponent();
        BindingContext = this;
        GetUserEmail();
        AddBudgetsToPicker();
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        string selectedCategory = (string)picker.SelectedItem;
        DisplayAlert("Selection", $"You selected: {selectedCategory}", "OK");
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
            await DisplayAlert("Atention", "Error", "Ok");
            await Navigation.PopAsync();
        }
   else if (!(budgets.Count > 0))
        {
            await DisplayAlert("Atention", "You don't have any budgets in order to add element. Consider making one.", "Ok");
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
            await DisplayAlert("Atention", "The Name label is required! Consider filling it.", "Ok");
        }
        else if (String.IsNullOrEmpty(txtAmount.Text))
        {
            await DisplayAlert("Atention", "The Amount label is required! Consider filling it.", "Ok");
        }
        else if (String.IsNullOrEmpty(BudgetsPicker.SelectedItem.ToString()))
        {
            await DisplayAlert("Atention", "You should pick where to add this element! Consider picking it.", "Ok");
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
}