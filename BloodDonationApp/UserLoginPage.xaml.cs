using BloodDonationApp.Helpers;
using BloodDonationApp.Models;
using BloodDonationApp.Services;
namespace BloodDonationApp;

public partial class UserLoginPage : ContentPage
{
    private readonly IApiService _apiServices;

    public UserLoginPage(IApiService apiServices)
    {
        InitializeComponent();
        _apiServices = apiServices;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        LoginRequestDto loginRequest = new LoginRequestDto {
            Tc = TcEntry.Text,
            Password = PasswordEntry.Text
        };
        
        var result = await _apiServices.LoginAsync(loginRequest);

        if (result.Success)
        {
            SessionManager.SetUser(result.Data); 
            await Shell.Current.GoToAsync(nameof(UserHomePage));
        }
        else
        {
            await DisplayAlert("Hata", result.Message, "Tamam");
        }
    }
}
