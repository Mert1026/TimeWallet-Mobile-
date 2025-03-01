using TimeWallet_Mobile_.Data.Models;

namespace TimeWallet_Mobile_;

public partial class ViewDetailsPage : ContentPage
{

    private int _frameMaxWidth = 145;
    private int _frameMinWidth = 145;
    private int _frameNameSize = 15;
    private int _frameAmountLabesSize = 12;
    private int _frameDateLabelSize = 10;
    private int _frameBtnTextSize = 10;
    private int currentLevel = 1;

    private List<Elements> elementsList = new List<Elements>
        {
            new Elements
            {
                id = Guid.NewGuid(),
                BudgetId = Guid.NewGuid(),
                Budgets = new Budgets { /* Initialize properties if necessary */ },
                Name = "Groceries",
                Amount = 150.75m,
                CreatedAt = long.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                ReceiptId = 123

            },
            new Elements
            {
                id = Guid.NewGuid(),
                BudgetId = Guid.NewGuid(),
                Budgets = new Budgets { /* Initialize properties if necessary */ },
                Name = "Rent",
                Amount = 1200.00m,
                CreatedAt = long.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            },
            new Elements
            {
                id = Guid.NewGuid(),
                BudgetId = Guid.NewGuid(),
                Budgets = new Budgets { /* Initialize properties if necessary */ },
                Name = "Entertainment",
                Amount = 100.50m,
                CreatedAt = long.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            }
        };

    public ViewDetailsPage(List<Elements> elements)
	{
		InitializeComponent();
        GenerateElementsLayout(elementsList);
        UpdateFrameColors();

    }

    private void GenerateElementsLayout(List<Elements> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            var elementFrame = CreateElementFrame(elements[i]);
            ElementsLayout.Children.Add(elementFrame);
        }
    }

    private Frame CreateElementFrame(Elements element)
    {
        bool fromReceipt = element.ReceiptId!=null;

        var nameLabel = new Label
        {
            Text = element.Name,
            FontSize = _frameNameSize,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };

        var amountLabel = new Label
        {
            Text = $"US${element.Amount:F2}",
            FontSize = _frameAmountLabesSize,
            HorizontalOptions = LayoutOptions.Center
        };

        var dateLabel = new Label
        {
            Text = (element.CreatedAt).ToString("dd/MM/yyyy"),
            FontSize = _frameDateLabelSize,
            HorizontalOptions = LayoutOptions.Center
        };

        var deleteButton = new Button
        {
            Text = "DELETE",
            FontSize = _frameBtnTextSize,
            BackgroundColor = Colors.Red,
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
            var goToReceiptButton = new Button
            {
                Text = "Receipt",
                FontSize = _frameBtnTextSize,
                BackgroundColor = Colors.Green,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                Command = new Command(() => GoToReceipt(element.BudgetId))
            };

            var buttonsLayout = new HorizontalStackLayout
            {
                Children = { deleteButton, goToReceiptButton },
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 5 // Add a small spacing between buttons
            };

            layout.Children.Add(buttonsLayout);
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
            Padding = 5,  // Reduced padding
            CornerRadius = 10,
            Margin = 5,    // Reduced margin
            BackgroundColor = Colors.LightGray
        };
    }


    private void DeleteElement(Guid elementId)
    {
        Elements elToRemove = elementsList.FirstOrDefault(e => e.id == elementId);
        elementsList.Remove(elToRemove);
        ElementsLayout.Children.Clear();
        GenerateElementsLayout(elementsList);
        //Remove the element from your list and refresh the grid
    }

    private void GoToReceipt(Guid budgetId)
    {
        //Navigate to the receipt details page
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
        levelOne_frame.BackgroundColor = currentLevel >= 1 ? Colors.Green : Colors.Gray;
        levelTwo_frame.BackgroundColor = currentLevel >= 2 ? Colors.Green : Colors.Gray;
        levelThree_frame.BackgroundColor = currentLevel >= 3 ? Colors.Green : Colors.Gray;
    }

}