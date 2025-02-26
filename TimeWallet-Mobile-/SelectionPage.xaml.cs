using TimeWallet_Mobile_.Data.Models;

namespace TimeWallet_Mobile_;

public partial class SelectionPage : ContentPage
{

    private VerticalStackLayout buttonContainer;

    private List<Budgets> _budgets;

    public SelectionPage(List<Budgets> budgets)
	{
		InitializeComponent();
        _budgets = budgets;

        buttonContainer = new VerticalStackLayout { Padding = 20 };
        Content = new ScrollView { Content = buttonContainer };

        foreach (var item in budgets)
        {
            Button button = new Button
            {
                Text = item.Name,
                TextColor = Colors.Black,
                FontSize = 16,
                BackgroundColor = Colors.LightGray,
                Margin = new Thickness(5),
            };

            button.Clicked += (sender, e) => OnButtonClicked(item.Name); // Pass name to method

            buttonContainer.Children.Add(button);
        }

    }
    private async void OnButtonClicked(string budgetName)
    {
        buttonContainer.Children.Clear();
        buttonContainer.Children.Add(new Label { Text = budgetName });
        await Task.Delay(1000);
        await Navigation.PushAsync(new CameraPage(true));


    }
}