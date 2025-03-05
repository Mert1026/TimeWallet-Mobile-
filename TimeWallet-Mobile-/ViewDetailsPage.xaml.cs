using Newtonsoft.Json;
using System.Collections.Generic;
using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.Models;
using TimeWallet_Mobile_.Data.Translation;

namespace TimeWallet_Mobile_;

public partial class ViewDetailsPage : ContentPage
{

    private ApiService _apiService;
    private Translations _translations;
    private Guid _budgedId;

    private int _frameMaxWidth = 145;
    private int _frameMinWidth = 145;
    private int _frameNameSize = 15;
    private int _frameAmountLabesSize = 12;
    private int _frameDateLabelSize = 10;
    private int _frameBtnTextSize = 10;
    private int currentLevel = 1;

    private List<Elements> elementsList = new List<Elements>();

    public ViewDetailsPage(List<Elements> elements, Guid budgetId)
	{
        _apiService = new ApiService();
        _budgedId = budgetId;
		elementsList.AddRange(elements);
        _translations = new Translations("bg");
        SetLanguage();
        InitializeComponent();
        GenerateElementsLayout(elementsList);
        ButtonsCalibration();
        SetLanguage();
        UpdateFrameColors();
        Shell.SetTabBarIsVisible(this, false);

    }

    private void GenerateElementsLayout(List<Elements> elements)
    {
        ButtonsCalibration();
        for (int i = 0; i < elements.Count; i++)
        {
            var elementFrame = CreateElementFrame(elements[i]);
            ElementsLayout.Children.Add(elementFrame);
            SetLanguage();
        }
    }

    private Frame CreateElementFrame(Elements element)
    {
        ButtonsCalibration();
        bool fromReceipt = element.ReceiptId!=null;

        var nameLabel = new Label
        {
            Text = element.Name,
            TextColor=Color.FromArgb("#0a5d40"),
            FontSize = _frameNameSize,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };

        var amountLabel = new Label
        {
            Text = $"US${element.Amount:F2}",
            TextColor = Color.FromArgb("#0a5d40"),
            FontSize = _frameAmountLabesSize,
            HorizontalOptions = LayoutOptions.Center
        };

        var dateLabel = new Label
        {
            Text = (DateTimeOffset.FromUnixTimeMilliseconds(element.CreatedAt).UtcDateTime).ToString("dd/MM/yyyy"),
            TextColor = Color.FromArgb("#0a5d40"),
            FontSize = _frameDateLabelSize,
            HorizontalOptions = LayoutOptions.Center
        };

        var deleteButton = new Button
        {
            Text = $"{_translations.DeleteLabel}",
            FontSize = _frameBtnTextSize,
            BackgroundColor = Color.FromArgb("f51e1e"),
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Command = new Command(() => DeleteElement(element.id))
        };

        var layout = new VerticalStackLayout
        {
            Children = { nameLabel, amountLabel, dateLabel },
            Spacing = 5  // Add a small spacing between items in the vertical stack
        };

        if (fromReceipt)
        {
            nameLabel.Text = $"{nameLabel.Text} - 🧾";
            layout.Children.Add(deleteButton);
        }
        else
        {
            layout.Children.Add(deleteButton);
        }

        return new Frame
        {
            Content = layout,
            MaximumWidthRequest = _frameMaxWidth,
            MinimumWidthRequest = _frameMinWidth,
            HorizontalOptions = LayoutOptions.Center,
            HasShadow = true,
            Padding = 5,  // Reduced padding
            CornerRadius = 10,
            Margin = 5,    // Reduced margin
            BackgroundColor = Color.FromArgb("#e1f2d9"),
            Shadow = new Shadow
            {
                Brush = Color.FromRgba(0, 0, 0, 0.4), // Shadow color (black with 40% opacity)
                Offset = new Point(5, 5), // Shadow position offset (X, Y)
                Radius = 20, // Blur radius of the shadow
                Opacity = 1 // Shadow transparency
            }
        };
    }


    private async void DeleteElement(Guid elementId)
    {
        Elements elToRemove = elementsList.FirstOrDefault(e => e.id == elementId);
        string email = await SecureStorage.GetAsync("UserEmail");
        await _apiService.DeleteElementAsync(email, elementId.ToString());
        elementsList.Remove(elToRemove);
        ElementsLayout.Children.Clear();
        GenerateElementsLayout(elementsList);
        //Remove the element from your list and refresh the grid
    }

    private void plusBtn_Clicked(object sender, EventArgs e)
    {
        if (currentLevel < 3)
        {
            currentLevel++;
            UpdateFrameColors();
            _frameMaxWidth += 35;
            _frameMinWidth += 35;
            _frameNameSize += 5;
            _frameAmountLabesSize += 5;
            _frameDateLabelSize += 5;
            _frameBtnTextSize += 5;
            ElementsLayout.Children.Clear();
            GenerateElementsLayout(elementsList);


        }
    }

    private void minusBtn_Clicked(object sender, EventArgs e)
    {
        if (currentLevel > 1)
        {
            currentLevel--;
            UpdateFrameColors();
            _frameMaxWidth -= 35;
            _frameMinWidth -= 35;
            _frameNameSize -= 5;
            _frameAmountLabesSize -= 5;
            _frameDateLabelSize -= 5;
            _frameBtnTextSize -= 5;
            ElementsLayout.Children.Clear();
            GenerateElementsLayout(elementsList);
        }
    }

    private void UpdateFrameColors()
    {
        levelOne_frame.BackgroundColor = currentLevel >= 1 ? Color.FromArgb("#0a5c41") : Color.FromArgb("#e1f3d8");
        levelTwo_frame.BackgroundColor = currentLevel >= 2 ? Color.FromArgb("#0a5c41") : Color.FromArgb("#e1f3d8");
        levelThree_frame.BackgroundColor = currentLevel >= 3 ? Color.FromArgb("#0a5c41") : Color.FromArgb("#e1f3d8");
    }

    private async void Refresh_btn_Clicked(object sender, EventArgs e)
    {
        OnAppearing();
        await RefreshData();
    }

    private async void ButtonsCalibration()
    {
        string theme = await SecureStorage.GetAsync("theme");
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

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshData();
        ButtonsCalibration();
        // Refresh data when the page appears
    }

    private async void goBack_Btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async Task RefreshData()
    {
        string email = await SecureStorage.GetAsync("UserEmail");

        if (email != null)
        {
            // Fetch the latest elements from API
            var userBudgetResponse = await _apiService.GetInformationAboutUser(email);
            List<Elements> elements = JsonConvert.DeserializeObject<List<Elements>>(userBudgetResponse.elementJson);

            if (elements != null)
            {
                elementsList.Clear(); // Clear the old data
                ElementsLayout.Children.Clear(); // Clear the UI elements

                elementsList.AddRange(elements); // Add new data
                GenerateElementsLayout(elementsList); // Regenerate the UI
            }
        }
    }

    public async void SetLanguage()
    {
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
    }

}