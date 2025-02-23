namespace TimeWallet_Mobile_;
using TimeWallet_Mobile_.Data.API_service;
using Newtonsoft.Json;
using TimeWallet_Mobile_.Data.Models;

public partial class NewPage1 : ContentPage
{
    private readonly ApiService _apiService;
    private readonly string _userEmail = "memo@gmail.com"; // Replace with actual email fetching logic


    public NewPage1()
	{
		InitializeComponent();
        _apiService = new ApiService();
        LoadBudgets();
    }

    private async void LoadBudgets()
    {
        try
        {
            var userBudgetResponse = await _apiService.GetInformationAboutUser(_userEmail);
            var budgets = JsonConvert.DeserializeObject<List<Budgets>>(userBudgetResponse.budgetJson);
            var elements = JsonConvert.DeserializeObject<List<Elements>>(userBudgetResponse.elementJson);

            decimal totalBudgeted = 0;
            decimal totalSpent = 0;

            BudgetLayout.Children.Clear();

            foreach (var budget in budgets)
            {
                decimal spent = elements.Where(e => e.BudgetId == budget.Id).Sum(e => e.Amount);
                totalBudgeted += budget.Amount;
                totalSpent += spent;

                BudgetLayout.Children.Add(CreateBudgetBox(budget.Name, budget.Amount, spent));
            }

            // Add total summary
            SummaryBudgeted.Text = $"{totalBudgeted:F2}";
            SummarySpent.Text = $"{totalSpent:F2}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private Frame CreateBudgetBox(string name, decimal budgeted, decimal spent)
    {
        return new Frame
        {
            Content = new VerticalStackLayout
            {
                Children =
                {
                    new Label { Text = name, FontAttributes = FontAttributes.Bold, FontSize = 18, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = $"US${budgeted:F2} Budgeted", FontSize = 14, HorizontalOptions = LayoutOptions.Center },
                    new ProgressBar { Progress = (double)(spent / budgeted), HeightRequest = 5, BackgroundColor = Colors.LightGray, ProgressColor = Colors.Green },
                    new Label { Text = $"US${spent:F2} spent | US${(budgeted - spent):F2} remaining", FontSize = 12, HorizontalOptions = LayoutOptions.Center },
                    new Button { Text = "View Details", FontSize = 12, Command = new Command(() => ViewDetails(name)), BackgroundColor=Color.FromHex("#0a5d40") }
                }
            },
            BorderColor = Colors.DarkGreen,
            Padding = 10,
            CornerRadius = 10,
            BackgroundColor = Colors.LightGreen,
            WidthRequest = 200,
            HeightRequest = 150,
            Margin = new Thickness(10)
        };
    }

    private void ViewDetails(string budgetName)
    {
        DisplayAlert("Budget Details", $"Details for {budgetName}", "OK");
    }

    private async void LOGIN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
}