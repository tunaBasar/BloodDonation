using BloodDonationApp.Services;
using Microsoft.Extensions.Logging;

namespace BloodDonationApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Servisleri kaydet
        builder.Services.AddSingleton<IApiService, ApiService>();
        
        // Sayfaları kaydet - Shell ile kullanım için
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<UserLoginPage>();
        builder.Services.AddTransient<AdminLoginPage>();
        builder.Services.AddTransient<UserRegisterPage>();
        builder.Services.AddTransient<UserHomePage>();
        builder.Services.AddTransient<RequestPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}