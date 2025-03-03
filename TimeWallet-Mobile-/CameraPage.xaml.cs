using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls.Compatibility;
using Newtonsoft.Json;
using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.DTO_s.Json;
using TimeWallet_Mobile_.Data.Models;
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
    
    private string _receiptId;

    public CameraPage(bool access)
    {
        if(access)
        {
            Navigation.PopAsync();
        }
        
        InitializeComponent();

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
            await DisplayAlert("Error", "Camera permission is required to use this feature.", "OK");
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

        var userBudgetResponse = await _apiService.GetInformationAboutUser(await SecureStorage.GetAsync("UserEmail"));
        _userBudgets = JsonConvert.DeserializeObject<List<Budgets>>(userBudgetResponse.budgetJson);
        string email = await SecureStorage.GetAsync("UserEmail");

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

                        await DisplayAlert("Barcode Detected", first.Value, "OK");
                        _receiptId = parts[1];
                        ReceiptDTO receiptInfo = await _apiService.GetReceiptAsync(email, _receiptId);
                        if(receiptInfo.Receipt == null)
                        {
                            await DisplayAlert("Atention", "There is no receipt in existence with the following parameters!", "Ok");
                            await Navigation.PopAsync();
                        }
                        UserDTO userInfo = await _apiService.GetUserAsync(email);
                        UsersReceiptsDTO receiptToAdd = new UsersReceiptsDTO()
                        {
                            id = receiptInfo.Receipt.id,
                            ShopId = receiptInfo.Receipt.ShopId,
                            ShopImage = receiptInfo.Receipt.ShopImage,
                            DateTime = receiptInfo.Receipt.createdAt,
                            TotalAmount = receiptInfo.Receipt.TotalAmount,
                            UserId = userInfo.User.Id,
                        };
                        ClearPage();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Invalid barcode format.", "OK");
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

    private void ClearPage()
    {
        // Remove all children from the main layout
        if (Content is Layout<View> layout)
        {
            layout.Children.Clear();
        }

        Label MainText = new Label()
        {
            Text = "Select Budget:",
            FontSize = 27,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#0a5c41"),
            Margin = 20,
            HorizontalOptions = LayoutOptions.Center,
        };
        _optionsLayout.Children.Add(MainText);

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

            button.Clicked += (sender, e) => OnButtonClicked(item.Id); // Pass name to method

           _optionsLayout.Children.Add(button);
        }
        // Alternatively, set Content to an empty layout
        _viewToDisplay.Content = _optionsLayout;
        Content = _viewToDisplay;
        
    }

    private async void OnButtonClicked(Guid budgetId)
    {
        if (Content is Layout<View> layout)
        {
            layout.Children.Clear();
        }
        await Task.Delay(1000);
        string email = await SecureStorage.GetAsync("UserEmail");
        ReceiptDTO receiptInfo = await _apiService.GetReceiptAsync(email, _receiptId);

        foreach (ReceiptItem item in receiptInfo.Items)
        {
            ElementDTO element = new ElementDTO
            {
                id = Guid.NewGuid(),
                name = item.Name,
                budgetId = budgetId,
                amount = item.Amount,
                createdAt =((DateTimeOffset)item.Receipts.createdAt).ToUnixTimeMilliseconds(),
                receiptId = receiptInfo.Receipt.id
            };

            await _apiService.AddElementAsync(element, email);
        }

        await DisplayAlert("Success", "Expenses added.", "Ok");
        await Navigation.PopAsync();

    }

}