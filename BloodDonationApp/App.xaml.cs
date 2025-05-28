using BloodDonationApp.Services;

namespace BloodDonationApp;

public partial class App : Application
{
    public static IApiService ApiService { get; private set; }

    public App(AppShell appShell, IApiService apiService)
    {
        InitializeComponent();
        ApiService = apiService;
        MainPage = appShell;
    }
}
