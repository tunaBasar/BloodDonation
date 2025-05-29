using BloodDonationApp.Services;

namespace BloodDonationApp;

public partial class MainPage : ContentPage
{
    private readonly IApiService _apiService;

    public MainPage(IApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnAdminLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AdminLoginPage));
    }

    private async void OnUserLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UserLoginPage));
    }

    private async void OnUserRegister(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UserRegisterPage));
    }
}