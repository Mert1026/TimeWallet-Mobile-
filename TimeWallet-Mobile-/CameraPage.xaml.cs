using TimeWallet_Mobile_.Data.API_service;
using ZXing;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace TimeWallet_Mobile_;

public partial class CameraPage : ContentPage
{
    private bool isProcessingBarcode = false;
    private readonly ApiService _apiService = new ApiService();
    private bool isTorchOn = false;

    public CameraPage()
    {
        InitializeComponent();

        BarcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions()
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
            AutoRotate = true,
        };
        InitializeCamera();
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


    private void BarcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        if (isProcessingBarcode) return; // Ignore if already processing a barcode

        var first = e.Results?.FirstOrDefault();
        if (first != null)
        {
            isProcessingBarcode = true; // Set flag to prevent multiple detections

            Dispatcher.DispatchAsync(async () =>
            {
                try
                {


                    List<string> parts = first.Value.Split('*').ToList();

                    if (parts.Count >= 5)
                    {
                        await DisplayAlert("Barcode Detected", first.Value, "OK");
                        await NextPageAsync(parts[1], parts[0], parts[2], parts[3], parts[4]);
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

        //ReceiptPlusItemsJSON receiptInfo = await _apiService.GetReceiptAsync(email, receiptId);

        //List<(string, decimal)> itemss = receiptInfo.Items.Select(i => (i.Name, i.Amount)).ToList();

        //await Navigation.PushAsync(new ReceiptPage($"{receiptInfo.Receipt.ShopId}", "https://upload.wikimedia.org/wikipedia/commons/9/91/Lidl-Logo.svg", itemss, receiptInfo.Receipt.TotalAmount));
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
}