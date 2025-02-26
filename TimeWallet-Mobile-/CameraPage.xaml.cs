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
            return;
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



    private async Task NextPageAsync(string receiptId, string storeId, string date, string time, string amount)
    {
        //if (Application.Current.MainPage is NavigationPage navPage)
        //{
        //    await navPage.PushAsync(new ReceiptInfoPage(receiptId, storeId, dateTime));
        //}
        //else
        //{
        //    await Application.Current.MainPage.DisplayAlert("Navigation Error", "NavigationPage is missing.", "OK");
        //}
        string email = await SecureStorage.GetAsync("UserEmail");

        ReceiptDTO receiptInfo = await _apiService.GetReceiptAsync(email, _receiptId);

        List<Elements> elements = new List<Elements>();
        foreach(ReceiptItem item in receiptInfo.Items)
        {
            elements.Add(new Elements
            {
                Name = item.Name,
                //BudgetId
            });
        }

        //await Navigation.PushAsync();
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
            FlashLight_Btn.BackgroundColor = Color.FromHex("#e1f2d9");
            BarcodeReader.IsTorchOn = true;
            isTorchOn = true;
        }
        else
        {
            FlashLight_Btn.ImageSource = "torchoff.svg";
            FlashLight_Btn.BackgroundColor = Color.FromHex("#0a5c41");
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
        foreach (var item in _userBudgets)
        {
            Button button = new Button
            {
                Text = item.Name,
                TextColor = Colors.Black,
                FontSize = 16,
                BackgroundColor = Colors.LightGray,
                Margin = new Thickness(5),
            };

            button.Clicked += (sender, e) => OnButtonClicked(item.Id); // Pass name to method

           _optionsLayout.Children.Add(button);
        }
        // Alternatively, set Content to an empty layout
        Content = _optionsLayout;
        
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
                budgetId = budgetId.ToString(),
                amount = item.Amount,
                createdAt = item.Receipts.DateTime.ToString("MM/dd/yyyy"),
                receiptId = receiptInfo.Receipt.id
            };

            await _apiService.AddElementAsync(element, email);
        }
    }

}