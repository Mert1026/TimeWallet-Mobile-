using TimeWallet_Mobile_.Data.API_service;
using Newtonsoft.Json;
using TimeWallet_Mobile_.Data.Models;
using Microsoft.Maui.Graphics.Text;
using System.Xml.Linq;
using TimeWallet_Mobile_.Data.Translation;

namespace TimeWallet_Mobile_;

public partial class NewPage2 : ContentPage
{
    private readonly ApiService _apiService;
    private string _userEmail = ""; // Replace with actual email fetching logic
    private List<Elements> _elements = new List<Elements>();
    private int currentLevel = 1;

    private Translations _translations;

    private int _labelsFontsSizes = 8;


    private int _framesWidth = 210;
    private int _framesInnerWidth = 160;
    private int _framesHeight = 150;
    private int _nameLabelSize = 25;
    private int _infoLabelsSize = 10;
    private int _progressBarHeight = 6;
    private int _buttonTextSize = 17;
    private int _buttonsDimensions = 40;
    public NewPage2()
    {
        InitializeComponent();
        _apiService = new ApiService();
        ButtonsCalibration();
        double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        double targetWidth = screenWidth * 0.75;
        UpdateFrameColors();
        LoadBudgets();
    }

    private async void LoadBudgets()
    {
        _userEmail = await SecureStorage.GetAsync("UserEmail");
        try
        {
            _userEmail = await SecureStorage.GetAsync("UserEmail");
            var userBudgetResponse = await _apiService.GetInformationAboutUser(_userEmail);
            var budgets = JsonConvert.DeserializeObject<List<Budgets>>(userBudgetResponse.budgetJson);
            var elements = JsonConvert.DeserializeObject<List<Elements>>(userBudgetResponse.elementJson);
            _elements = elements;

            decimal totalBudgeted = 0;
            decimal totalSpent = 0;

            BudgetLayout.Children.Clear();

            foreach (var budget in budgets)
            {
                decimal spent = elements.Where(e => e.BudgetId == budget.Id).Sum(e => e.Amount);
                totalBudgeted += budget.Amount;
                totalSpent += spent;

                BudgetLayout.Children.Add(CreateBudgetBox(budget.Id, budget.Amount, spent, budget.Name));
            }

            // Add total summary
            SummaryBudgeted.Text = $"{totalBudgeted:F2}";
            SummarySpent.Text = $"{totalSpent:F2}";
        }
        catch (Exception ex)
        {
            await DisplayAlert(_translations.Error, ex.Message, _translations.OkText);
        }
    }

    private Frame CreateBudgetBox(Guid budgetId, decimal budgeted, decimal spent, string name)
    {
        string color = "#0a5d40";
        if(budgeted - spent == 0)
        {
            color = "#ffb739";
        }
        else if(budgeted - spent < 0)
        {
            color = "f51e1e";
        }
        return new Frame
        {
            WidthRequest = _framesWidth,
            VerticalOptions = LayoutOptions.Center,
            Content = new VerticalStackLayout
            {
                WidthRequest = _framesInnerWidth,
                Children =
                {
            new Label { Text = name, FontAttributes = FontAttributes.Bold, FontSize = _nameLabelSize, HorizontalOptions = LayoutOptions.Center, TextColor = Color.FromArgb("#0a5d40") },
            new Label { Text = $"US${budgeted:F2} {_translations.BudgetsSecondarytext}", FontSize = _infoLabelsSize, HorizontalOptions = LayoutOptions.Center, TextColor = Color.FromArgb("#0a5d40") },
            new ProgressBar { Progress = (double)(spent / budgeted), HeightRequest = _progressBarHeight, BackgroundColor = Color.FromArgb("#c5e7b3"), ProgressColor = Color.FromArgb(color) },
            new Label { Text = $"US${spent:F2} {_translations.BudgetsThirdlyText_One} | US${(budgeted - spent):F2} {_translations.BudgetsThirdlyText_Two}", FontSize = _infoLabelsSize, HorizontalOptions = LayoutOptions.Center, TextColor = Color.FromArgb(color), FontAttributes=FontAttributes.Bold },
            new HorizontalStackLayout
            {
                 new Button { Text = $"{_translations.BudgetsButtonText}", FontSize = _buttonTextSize+3, Command = new Command(() => ViewDetails(budgetId)), BackgroundColor=Color.FromArgb("#0a5d40"), Margin=5, Padding=0, TextColor=Color.FromArgb("#e1f2d9"), WidthRequest = _buttonsDimensions+60, HeightRequest = _buttonsDimensions},
                 new ImageButton {Source = "trash.svg",Command = new Command(() => DeleteBudget(budgetId)), BackgroundColor=Color.FromArgb("#f51e1e"), Margin=5, Padding=5, WidthRequest = _buttonsDimensions, CornerRadius = 10, HeightRequest = _buttonsDimensions}
                
            },
                }
            },
            BorderColor = Color.FromArgb("#15260d"),
            Padding = 10,
            CornerRadius = 10,
            BackgroundColor = Color.FromArgb("#e1f3d8"),
            HeightRequest = _framesHeight,
            HasShadow= true,
            Margin = 10,

            //Adding Shadow
            Shadow = new Shadow
            {
                Brush = Color.FromRgba(0, 0, 0, 0.4), // Shadow color (black with 40% opacity)
                Offset = new Point(5, 5), // Shadow position offset (X, Y)
                Radius = 20, // Blur radius of the shadow
                Opacity = 1 // Shadow transparency
            }
        };
    }


    private async void ViewDetails(Guid budgetId)
    {
        var userBudgetResponse = await _apiService.GetInformationAboutUser(_userEmail);
        var elements = JsonConvert.DeserializeObject<List<Elements>>(userBudgetResponse.elementJson);
        _elements = elements;
        await Navigation.PushAsync(new ViewDetailsPage(_elements.Where(e => e.BudgetId == budgetId).ToList(), budgetId));

    }

    private async void LOGIN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private async void DeleteBudget(Guid budgetId)
    {
        string response = await _apiService.DeleteBudgetAsync(_userEmail, budgetId.ToString());
        string userName = await SecureStorage.GetAsync("UserName");
        if (response == $"User({userName}) doesn't own the provided budget!")
        {
            await DisplayAlert(_translations.Atention, _translations.DontOwnBudget, _translations.OkText);
        }
        else if (response == "Something went wrong, please try again.")
        {
            await DisplayAlert(_translations.Atention, _translations.Error, _translations.OkText);
        }
        else
        {
            await DisplayAlert(_translations.Success, _translations.SuccessDeletedBudged, _translations.OkText);
            BudgetLayout.Children.Clear();
            LoadBudgets();
        }
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
            _buttonsDimensions += 15;
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
            _buttonsDimensions -= 15;
            LoadBudgets();
        }
    }

    private void UpdateFrameColors()
    {
        levelOne_frame.BackgroundColor = currentLevel >= 1 ? Color.FromArgb("#0a5c41") : Color.FromArgb("#e1f3d8");
        levelTwo_frame.BackgroundColor = currentLevel >= 2 ? Color.FromArgb("#0a5c41") : Color.FromArgb  ("#e1f3d8");
        levelThree_frame.BackgroundColor = currentLevel >= 3 ? Color.FromArgb("#0a5c41") : Color.FromArgb("#e1f3d8");
    }

    private void Refresh_btn_Clicked(object sender, EventArgs e)
    {
        OnAppearing();
        LoadBudgets();
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
        spent_Sign.Text = _translations.BudgetsThirdlyText_One;
        budgeted_Sign.Text = _translations.BudgetsSecondarytext;
        currentBalance_Sign_1.Text = _translations.BudgetsCurrentBalance;
        currentBalance_Sign_2.Text = _translations.BudgetsCurrentBalance;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        BudgetLayout.Children.Clear();
        ButtonsCalibration();
        LoadBudgets();
    }
}
