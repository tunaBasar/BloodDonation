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
        if (string.IsNullOrWhiteSpace(TcEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Hata", "TC ve şifre alanları boş bırakılamaz.", "Tamam");
            return;
        }

        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        LoginButton.IsEnabled = false;

        try
        {
            var loginRequest = new LoginRequestDto
            {
                Tc = TcEntry.Text,
                Password = PasswordEntry.Text
            };

            var response = await _apiServices.LoginAsync(loginRequest);

            if (response.Success)
            {
                var userSession = App.Current.Services.GetService<SessionManager>();
                userSession.CurrentUser = response.Data;

                await Shell.Current.GoToAsync($"//{nameof(UserHomePage)}");
            }
            else
            {
                await DisplayAlert("Hata", response.Message, "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Giriş sırasında hata oluştu: {ex.Message}", "Tamam");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            LoginButton.IsEnabled = true;
        }
    }
}