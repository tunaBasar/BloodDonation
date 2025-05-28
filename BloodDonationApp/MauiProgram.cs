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

#if DEBUG
		builder.Logging.AddDebug();
		builder.Services.AddSingleton<IApiService, ApiService>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<App>();

		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<UserLoginPage>();
		builder.Services.AddTransient<AdminLoginPage>();
		builder.Services.AddTransient<UserRegisterPage>();
		builder.Services.AddTransient<UserHomePage>();
#endif

		return builder.Build();
	}
}
