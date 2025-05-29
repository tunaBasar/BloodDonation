namespace BloodDonationApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(UserLoginPage), typeof(UserLoginPage));
        Routing.RegisterRoute(nameof(UserRegisterPage), typeof(UserRegisterPage));
        Routing.RegisterRoute(nameof(AdminLoginPage), typeof(AdminLoginPage));
        Routing.RegisterRoute(nameof(UserHomePage), typeof(UserHomePage));
        Routing.RegisterRoute(nameof(RequestPage), typeof(RequestPage));
        Routing.RegisterRoute(nameof(UserDonationPage), typeof(UserDonationPage));
    }
}