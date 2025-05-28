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
        await Navigation.PushAsync(new AdminLoginPage());
    }

    private async void OnUserLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserLoginPage(_apiService));
    }

    private async void OnUserRegister(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserRegisterPage());
    }
}
