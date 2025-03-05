using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.DTO_s.Json;
using TimeWallet_Mobile_.Data.Models;

namespace TimeWallet_Mobile_;

public partial class RegisterPage : ContentPage
{
    private readonly ApiService _apiservice = new ApiService();

    public RegisterPage()
    {
        InitializeComponent();
    }

    // private ApplicationDbContext context = new ApplicationDbContext();

    private async void RegisterBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Create the UserDTO with the user details from the UI fields

            User user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = txtName.Text, // Assume you have an Entry for Name
                Email = txtEmail.Text, // Assume you have an Entry for Email
                UserName = txtName.Text, // Assume you have an Entry for Username
                PasswordHash = txtPassword.Text, // Assume you have an Entry for Password
            };
            

            // Call the API to register the user
            string result = await _apiservice.RegisterUserAsync(user);

            // Show a result message
            if (result.Contains("Error"))
            {
                await DisplayAlert("Registration Failed", result, "OK");
            }
            else
            {
                await DisplayAlert("Registration Successful", result, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Something went wrong: " + ex.Message, "OK");
        }

    }


    private async void OnHaveAnAccount_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
}