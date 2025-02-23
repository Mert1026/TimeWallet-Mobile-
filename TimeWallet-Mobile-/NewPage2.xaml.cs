using TimeWallet_Mobile_.Data.API_service;
using Newtonsoft.Json;
using TimeWallet_Mobile_.Data.Models;
using Microsoft.Maui.Graphics.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace TimeWallet_Mobile_;

public partial class NewPage2 : ContentPage
{
    private readonly ApiService _apiService;
    private readonly string _userEmail = "memo@gmail.com"; // Replace with actual email fetching logic
    private List<Budgets> _budgets = new List<Budgets>();
    private List<Elements> _elements = new List<Elements>();
    private int currentLevel = 1;

    private int _labelsFontsSizes = 8;


    private int _framesWidth = 210;
    private int _framesInnerWidth = 160;
    private int _framesHeight = 160;
    private int _nameLabelSize = 25;
    private int _infoLabelsSize = 10;
    private int _progressBarHeight = 6;
    private int _buttonTextSize = 18;
    public NewPage2()
    {
        InitializeComponent();
        addThings();
        _apiService = new ApiService();
        double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        double targetWidth = screenWidth * 0.75;
        UpdateFrameColors();
        LoadBudgets();
    }

    private async void LoadBudgets()
    {
        try
        {
            //var userBudgetResponse = await _apiService.GetInformationAboutUser(_userEmail);
            var budgets = _budgets;
            var elements = _elements;

            decimal totalBudgeted = 200;
            decimal totalSpent = 700;

            BudgetLayout.Children.Clear();

            foreach (var budget in budgets)
            {

                BudgetLayout.Children.Add(CreateBudgetBox(budget.Name, budget.Amount, (decimal)300.00));
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
            WidthRequest = _framesWidth,
            VerticalOptions = LayoutOptions.Center,
            Content = new VerticalStackLayout
            {
                WidthRequest = _framesInnerWidth,
                Children =
                {
                    new Label { Text = name, FontAttributes = FontAttributes.Bold, FontSize = _nameLabelSize, HorizontalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#0a5d40") },
                    new Label { Text = $"US${budgeted:F2} Budgeted", FontSize = _infoLabelsSize, HorizontalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#0a5d40") },
                    new ProgressBar { Progress = (double)(spent / budgeted), HeightRequest = _progressBarHeight, BackgroundColor = Color.FromHex("#c5e7b3"), ProgressColor = Color.FromHex("#0a5d40") },
                    new Label { Text = $"US${spent:F2} spent | US${(budgeted - spent):F2} remaining", FontSize = _infoLabelsSize, HorizontalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#0a5d40") },
                    new Button { Text = "View Details", FontSize = _buttonTextSize, Command = new Command(() => ViewDetails(name)), BackgroundColor=Color.FromHex("#0a5d40"), Margin=10, TextColor=Colors.White }
                }
            },
            BorderColor = Colors.DarkGreen,
            Padding = 10,
            CornerRadius = 10,
            BackgroundColor = Color.FromHex("#e1f3d8"),
            HeightRequest = _framesHeight,
            Margin = 10
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

    private void addThings()
    {
        

        Budgets budget1 = new Budgets
        {
            Id = Guid.Parse("48fd5733-98b6-429c-9928-75316e975753"),
            UserId = "ba518011-61f4-4eff-aecf-73da127a4c46",
            User = null,
            Name = "House",
            CreatedAt = DateTime.Parse("2025-02-14T05:30:08.413"),
            Amount = 750.00m
        };
        _budgets.Add(budget1);

        Budgets budget2 = new Budgets
        {
            Id = Guid.Parse("5be4be41-ffe9-410e-80c4-d3fa5755b271"),
            UserId = "ba518011-61f4-4eff-aecf-73da127a4c46",
            User = null,
            Name = "efsdf",
            CreatedAt = DateTime.Parse("2025-02-18T14:59:50.026"),
            Amount = 54.00m
        };
        _budgets.Add(budget2);

        Budgets budget3 = new Budgets
        {
            Id = Guid.Parse("eab6bad8-0b34-46fc-a867-f20c71725838"),
            UserId = "ba518011-61f4-4eff-aecf-73da127a4c46",
            User = null,
            Name = "Family",
            CreatedAt = DateTime.Parse("2025-02-14T05:30:30.611"),
            Amount = 850.00m
        };
        _budgets.Add(budget3);

        Budgets budget4 = new Budgets
        {
            Id = Guid.Parse("1fe906a0-e1d5-4181-92ae-f6bc29ae9172"),
            UserId = "ba518011-61f4-4eff-aecf-73da127a4c46",
            User = null,
            Name = "Car",
            CreatedAt = DateTime.Parse("2025-02-14T05:29:51.329"),
            Amount = 2500.00m
        };
        _budgets.Add(budget4);

        // Manually adding Elements
        List<Elements> elements = new List<Elements>();

        Elements element1 = new Elements
        {
            id = Guid.Parse("761f7438-acd7-44e3-8609-2367429661ec"),
            BudgetId = budget3.Id,
            Budgets = budget3,
            Name = "test",
            Amount = 55.00m,
            CreatedAt = "02/14/2025"
        };
        _elements.Add(element1);

        Elements element2 = new Elements
        {
            id = Guid.Parse("9ad8f6e9-d1c9-4a57-8718-735c65a681d2"),
            BudgetId = budget4.Id,
            Budgets = budget4,
            Name = "Tires",
            Amount = 250.00m,
            CreatedAt = "02/14/2025"
        };
        _elements.Add(element2);

        Elements element3 = new Elements
        {
            id = Guid.Parse("d2e95ef3-09f1-44dd-a529-8dfd1f9d8b32"),
            BudgetId = budget4.Id,
            Budgets = budget4,
            Name = "wow",
            Amount = 55.00m,
            CreatedAt = "02/14/2025"
        };
        _elements.Add(element3);

        Elements element4 = new Elements
        {
            id = Guid.Parse("ff743d26-f768-46f6-899a-92cea97ee52f"),
            BudgetId = budget1.Id,
            Budgets = budget1,
            Name = "kibidi",
            Amount = 55.00m,
            CreatedAt = "02/14/2025"
        };
        _elements.Add(element4);

        Elements element5 = new Elements
        {
            id = Guid.Parse("7d9cab38-573c-4455-bf89-a2d4d66998df"),
            BudgetId = budget1.Id,
            Budgets = budget1,
            Name = "test_2",
            Amount = 55.00m,
            CreatedAt = "02/14/2025"
        };
        _elements.Add(element5);

        Elements element6 = new Elements
        {
            id = Guid.Parse("b23fafd3-1caf-4aa5-93bd-a5f2f00fdd91"),
            BudgetId = budget1.Id,
            Budgets = budget1,
            Name = "Food",
            Amount = 450.00m,
            CreatedAt = "02/14/2025"
        };
        _elements.Add(element6);
    }

    private void UpdateSummaryFontsSizes(int labelsFontSizes)
    {
        budgeted_Sign.FontSize = labelsFontSizes + 4;
        SummaryBudgeted.FontSize = labelsFontSizes + 12;
        currentBalance_Sign_1.FontSize = labelsFontSizes;

        spent_Sign.FontSize = labelsFontSizes + 4;
        SummarySpent.FontSize = labelsFontSizes + 12;
        currentBalance_Sign_2.FontSize = labelsFontSizes;

    }

    private void plusBtn_Clicked(object sender, EventArgs e)
    {
        if (currentLevel < 3)
        {
            currentLevel++;
            UpdateFrameColors();
            _labelsFontsSizes += 4;
            UpdateSummaryFontsSizes(_labelsFontsSizes);
            BudgetLayout.Children.Clear();
            _framesHeight += 30;
            _framesInnerWidth += 30;
            _framesWidth += 30;
            _infoLabelsSize += 3;
            _nameLabelSize += 3;
            _buttonTextSize += 3;
            _progressBarHeight += 1;
            LoadBudgets();
        }
    }

    private void minusBtn_Clicked(object sender, EventArgs e)
    {
        if (currentLevel > 1)
        {
            currentLevel--;
            UpdateFrameColors();
            _labelsFontsSizes -= 4;
            UpdateSummaryFontsSizes(_labelsFontsSizes);
            BudgetLayout.Children.Clear();
            _framesHeight -= 30;
            _framesInnerWidth -= 30;
            _framesWidth -= 30;
            _infoLabelsSize -= 3;
            _nameLabelSize -= 3;
            _buttonTextSize -= 3;
            _progressBarHeight -= 1;
            LoadBudgets();
        }
    }

    private void UpdateFrameColors()
    {
        levelOne_frame.BackgroundColor = currentLevel >= 1 ? Colors.Green : Colors.Gray;
        levelTwo_frame.BackgroundColor = currentLevel >= 2 ? Colors.Green : Colors.Gray;
        levelThree_frame.BackgroundColor = currentLevel >= 3 ? Colors.Green : Colors.Gray;
    }

}



//<VerticalStackLayout Grid.Column="0" HorizontalOptions= "Center" VerticalOptions= "Center" >
//                < Label x:Name= "budgeted_Sign" Text= "Budgeted" FontAttributes= "Bold" FontSize= "16" TextColor= "Black" HorizontalTextAlignment= "Center" />
//                < Label x:Name= "SummaryBudgeted" FontSize= "24" FontAttributes= "Bold" HorizontalTextAlignment= "Center" />
//                < Label x:Name= "CurrentBalance_Sign_1" Text= "Current Balance" FontSize= "12" HorizontalTextAlignment= "Center" />
//            </ VerticalStackLayout >

//            < VerticalStackLayout Grid.Column= "1" HorizontalOptions= "Center" VerticalOptions= "Center" >
//                < Label x:Name= "spent_Sign" Text= "Spent" FontAttributes= "Bold" FontSize= "16" HorizontalTextAlignment= "Center" />
//                < Label x:Name= "SummarySpent" FontSize= "24" FontAttributes= "Bold" HorizontalTextAlignment= "Center" />
//                < Label x:Name= "currentBalance_Sign_2" Text= "Current Balance" FontSize= "12" HorizontalTextAlignment= "Center" />
//            </ VerticalStackLayout >
