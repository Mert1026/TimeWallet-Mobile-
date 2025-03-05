
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls.Compatibility;
using Newtonsoft.Json;
using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.DTO_s.Json;
using TimeWallet_Mobile_.Data.Models;
using TimeWallet_Mobile_.Data.Translation;
using ZXing;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;


namespace TimeWallet_Mobile_;

public partial class CameraPage : ContentPage
{
    private bool isProcessingBarcode = false;
    private readonly ApiService _apiService = new ApiService();
    private bool isTorchOn = false;
    private ScrollView _viewToDisplay;
    private VerticalStackLayout _optionsLayout;
    private List<Budgets> _userBudgets;
    private Translations _translations;

    private string _receiptId;

    public CameraPage(bool access)
    {
        if (access)
        {
            Navigation.PopAsync();
        }

        InitializeComponent();

        Shell.SetTabBarIsVisible(this, false);

        ButtonsCalibration();

        _optionsLayout = new VerticalStackLayout();
        _viewToDisplay = new ScrollView();

        BarcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions()
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
            AutoRotate = true,
        };
        InitializeCamera();

        BarcodeReader.IsDetecting = true; // Reactivate scanner
    }

    private async void InitializeCamera()
    {
        // Request camera permissions
        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert(_translations.Error, _translations.CameraPermissionError, _translations.Error);
            await Navigation.PopAsync();
        }
    }



    //private void BarcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    //{
    //    var first = e.Results?.FirstOrDefault();
    //    if (first != null)
    //    {
    //        Dispatcher.DispatchAsync(async () =>
    //        {
    //            await DisplayAlert("Barcode Detected", first.Value, "OK");

    //            List<string> parts = first.Value.Split('*').ToList();

    //            if (parts.Count >= 3)
    //            {
    //                await NextPageAsync(parts[1], parts[0], parts[2], parts[3], parts[4]);
    //            }
    //            else
    //            {
    //                await DisplayAlert("Error", "Invalid barcode format.", "OK");
    //            }
    //        });
    //    }
    //}


    private async void BarcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        if (isProcessingBarcode) return; // Ignore if already processing a barcode

        isProcessingBarcode = true; // Set flag to prevent multiple detections
        string email = await SecureStorage.GetAsync("UserEmail");
        var userBudgetResponse = await _apiService.GetInformationAboutUser(email);
        _userBudgets = JsonConvert.DeserializeObject<List<Budgets>>(userBudgetResponse.budgetJson);

        if(_userBudgets.Count <= 0)
        {
            await DisplayAlert(_translations.Atention, _translations.DontHaveAnyBudgets, _translations.OkText);
            await Navigation.PopAsync(); return;
        }
        
        var first = e.Results?.FirstOrDefault();

        if (first != null)
        {
            await Dispatcher.DispatchAsync(async () =>
            {
                try
                {
                    List<string> parts = first.Value.Split('*').ToList();

                    if (parts.Count >= 5)
                    {
                        // **Disable barcode scanner**
                        var scanner = sender as CameraBarcodeReaderView;
                        if (scanner != null)
                        {
                            scanner.IsDetecting = false; // Stop detecting barcodes
                        }

                        _receiptId = parts[1];
                        ReceiptDTO receiptInfo = await _apiService.GetReceiptAsync(email, _receiptId);
                        if (receiptInfo.Receipt == null)
                        {
                            await DisplayAlert(_translations.Atention,_translations.DontHaveReceiptExisting , _translations.OkText);
                            await Navigation.PopAsync();
                        }
                        UserDTO userInfo = await _apiService.GetUserAsync(email);
                        //UsersReceiptsDTO receiptToAdd = new UsersReceiptsDTO()
                        //{
                        //    id = receiptInfo.Receipt.id,
                        //    ShopId = receiptInfo.Receipt.ShopId,
                        //    ShopImage = "s",
                        //    DateTime = receiptInfo.Receipt.createdAt,
                        //    TotalAmount = receiptInfo.Receipt.TotalAmount,
                        //    UserId = userInfo.user.Id,
                        //};
                        //await _apiService.AddUserReceiptAsync(email, receiptToAdd);
                        ClearPage();
                    }
                    else
                    {
                        await DisplayAlert(_translations.Error, _translations.ErrorBarcode, _translations.OkText);
                    }
                }
                finally
                {
                    isProcessingBarcode = false; // Reset flag after processing
                }
            });
        }
    }




    private async void goBack_Btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
        BarcodeReader.IsTorchOn = false;
    }

    private void FlashLight_Btn_Clicked(object sender, EventArgs e)
    {
        if (!isTorchOn)
        {
            FlashLight_Btn.ImageSource = "torchon.svg";
            FlashLight_Btn.BackgroundColor = Color.FromArgb("#e1f2d9");
            BarcodeReader.IsTorchOn = true;
            isTorchOn = true;
        }
        else
        {
            FlashLight_Btn.ImageSource = "torchoff.svg";
            FlashLight_Btn.BackgroundColor = Color.FromArgb("#0a5c41");
            BarcodeReader.IsTorchOn = false;
            isTorchOn = false;
        }
    }

    private async void ClearPage()
    {
        ButtonsCalibration();
        // Remove all children from the main layout
        string email = await SecureStorage.GetAsync("UserEmail");
        ReceiptDTO receiptInfo = await _apiService.GetReceiptAsync(email, _receiptId);

        // Clear the layout(Първо!)
        if (Content is Layout<View> layout)
        {
            layout.Children.Clear();
        }

        // Create layout for holding all item views
        var itemLayouts = new VerticalStackLayout();

        //======= TITLE SECTION =======
        var titleLabel = new Label
        {
            Text = _translations.SelectionPageMainText,
            FontSize = 24,
            TextColor = Color.FromArgb("#0a5c41"),
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 10, 0, 10) // Spacing around the title
        };
        itemLayouts.Children.Add(titleLabel); 

        // A dictionary to map each item to the selected budged
        var itemToBudgetMap = new Dictionary<string, Guid>();

        
        foreach (var item in receiptInfo.Items)
        {
            var itemLayout = new HorizontalStackLayout
            {
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center
            };

            // Label for item name
            Label itemNameLabel = new Label
            {
                Text = item.Name + $" ({item.Amount.ToString()})",
                FontSize = 18,
                TextColor = Color.FromArgb("#0a5c41"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
            };

            // Picker inside a Frame for rounded corners and shadow
            var picker = new Picker
            {
                ItemsSource = _userBudgets.Select(b => b.Name).ToList(),
                SelectedIndex = -1,
                BackgroundColor = Colors.Transparent, // Transparent inside the frame
                TextColor = Color.FromArgb("#e1f2d9")
            };

            var pickerFrame = new Frame
            {
                CornerRadius = 10, // Rounded corners
                Padding = new Thickness(5),
                BackgroundColor = Color.FromArgb("#0a5c41"),
                BorderColor = Color.FromArgb("#0a5c41"),
                Shadow = new Shadow
                {
                    Brush = new SolidColorBrush(Colors.Black),
                    Offset = new Point(2, 2),
                    Opacity = (float)0.3,
                    Radius = 5
                },
                Content = picker
            };

            
            picker.SelectedIndexChanged += (sender, e) =>
            {
                var selectedBudgetName = picker.SelectedItem?.ToString();
                var selectedBudget = _userBudgets.FirstOrDefault(b => b.Name == selectedBudgetName);

                if (selectedBudget != null)
                {
                    
                    Console.WriteLine($"Selected Item ID: {item.id}");
                    Console.WriteLine($"Selected Budget ID: {selectedBudget.Id}");

                    
                    itemToBudgetMap[item.id] = selectedBudget.Id;
                }
                else
                {
                    
                    Console.WriteLine($"No budget found for: {selectedBudgetName}");
                }
            };


            itemLayout.Children.Add(itemNameLabel);
            itemLayout.Children.Add(pickerFrame); 
            itemLayouts.Children.Add(itemLayout);

            
            itemLayouts.Children.Add(new BoxView
            {
                Color = Color.FromArgb("#0a5c41"),
                HeightRequest = 1,
                HorizontalOptions = LayoutOptions.FillAndExpand, // Full width
                Margin = new Thickness(0, 10) // Space around the line
            });
        }

        // Create "Accomplish" button
        Button accomplishButton = new Button
        {
            Text = "Accomplish",
            BackgroundColor = Color.FromArgb("#0a5c41"),
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Margin = 10
        };

        accomplishButton.Clicked += async (sender, e) =>
        {
            // Loop through the items and handle budget assignment
            foreach (var item in receiptInfo.Items)
            {
                if (itemToBudgetMap.TryGetValue(item.id, out var selectedBudgetId))
                {
                    var selectedExpense = receiptInfo.Items.FirstOrDefault(expense => expense.id == item.id);
                    if (selectedExpense != null)
                    {
                        ElementDTO element = new ElementDTO
                        {
                            id = Guid.NewGuid(),
                            name = selectedExpense.Name,
                            budgetId = selectedBudgetId,
                            amount = selectedExpense.Amount,
                            createdAt = ((DateTimeOffset)selectedExpense.Receipts.createdAt).ToUnixTimeMilliseconds(),
                            receiptId = _receiptId
                        };

                        await _apiService.AddElementAsync(element, email);
                    }
                }
            }

            await DisplayAlert(_translations.Success, _translations.SuccessAddedExpenses, _translations.OkText);
            await Navigation.PopAsync();
        };

        ImageButton infoButton = new ImageButton
        {
            Source = "info.svg",
            HeightRequest = 50,
            WidthRequest = 50,
            CornerRadius = 10, 
            Padding = 5,
            BackgroundColor = Color.FromArgb("#215fe3"),
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(5, 10) // Keep equal margin for symmetry
        };

        // Display alert when clicked
        infoButton.Clicked += async (sender, e) =>
        {
            await DisplayAlert(_translations.SelectionPageInfoLabel, _translations.SelectionPageInfoText, _translations.OkText);
        };

        // Layout to arrange buttons horizontally
        var buttonLayout = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 10, // Space between buttons
            Children = { accomplishButton, infoButton }
        };

        // Add the buttons below the items layout
        itemLayouts.Children.Add(buttonLayout);
        Content = itemLayouts;
    }






    private async Task OnButtonClicked(Guid budgetId, Guid expenseId, int currentIndex)
    {
        // Clear the layout immediately before proceeding
        if (Content is Layout<View> layout)
        {
            layout.Children.Clear();
        }

        // Add the element to the selected budget for the expense
        string email = await SecureStorage.GetAsync("UserEmail");
        ReceiptDTO receiptInfo = await _apiService.GetReceiptAsync(email, _receiptId);

        // Find the expense being clicked
        var selectedExpense = receiptInfo.Items.FirstOrDefault(item => Guid.Parse(item.id) == expenseId);
        if (selectedExpense != null)
        {
            ElementDTO element = new ElementDTO
            {
                id = Guid.NewGuid(),
                name = selectedExpense.Name,
                budgetId = budgetId,
                amount = selectedExpense.Amount,
                createdAt = ((DateTimeOffset)selectedExpense.Receipts.createdAt).ToUnixTimeMilliseconds(),
                receiptId = receiptInfo.Receipt.id
            };

            await _apiService.AddElementAsync(element, email);
        }

        // Check if this is the last expense
        if (currentIndex == receiptInfo.Items.Count - 1)
        {
            // If it's the last expense, navigate back
            await Navigation.PopAsync();
        }
        else
        {
            // Move to the next expense
            // Create layout for the next expense, excluding the current one
            var nextExpense = receiptInfo.Items.Skip(currentIndex + 1).FirstOrDefault();
            if (nextExpense != null)
            {
                // Create label and button for the next expense
                Label MainText = new Label()
                {
                    Text = $"Select Budget for: {nextExpense.Name}",
                    FontSize = 27,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#0a5c41"),
                    Margin = 20,
                    HorizontalOptions = LayoutOptions.Center,
                };
                _optionsLayout.Children.Add(MainText);

                // Create and display buttons for each budget
                foreach (var item in _userBudgets)
                {
                    Button button = new Button
                    {
                        Text = item.Name,
                        TextColor = Color.FromArgb("#e1f2d9"),
                        FontSize = 16,
                        BackgroundColor = Color.FromArgb("#0a5c41"),
                        Margin = 10,
                    };

                    // Pass the budget and expense to the button click handler
                    button.Clicked += async (sender, e) => await OnButtonClicked(item.Id, Guid.Parse(nextExpense.id), currentIndex + 1);

                    _optionsLayout.Children.Add(button);
                }

                // Set the content to the generated layout for the next expense
                _viewToDisplay.Content = _optionsLayout;
                Content = _viewToDisplay;
            }
        }
    }

    private async void ButtonsCalibration()
    {
        string theme = await SecureStorage.GetAsync("Theme");
        if (theme == null)
        {
            await DisplayAlert("Atention", "Error occured! Try again later.", "Ok");
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
            await DisplayAlert("Atention", "Error occured! Try again later.", "Ok");
            await Navigation.PopAsync();
        }
        else if (language == "English")
        {
            _translations = new Translations("English");
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
        ButtonsCalibration();
        // Refresh data when the page appears
    }


}