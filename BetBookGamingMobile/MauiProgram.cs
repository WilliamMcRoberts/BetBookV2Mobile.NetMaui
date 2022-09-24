using BetBookGamingMobile.Services;
using BetBookGamingMobile.ViewModels;
using BetBookGamingMobile.Views;

namespace BetBookGamingMobile;

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

        builder.Services.AddHttpClient("vortex", client =>
        {
            client.BaseAddress = new Uri("https://user9f9bd262219b696.app.vtxhub.com/");
        });

        builder.Services.AddSingleton<IGameService, GameService>();
        builder.Services.AddTransient<ISingleBetService, SingleBetService>();
        builder.Services.AddTransient<IParleyBetSlipService, ParleyBetSlipService>();
        builder.Services.AddTransient<IUserService, UserService>();

        builder.Services.AddSingleton<BetSlipState>();

        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<GameDetailsViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<GameDetailsPage>();
        builder.Services.AddSingleton(Connectivity.Current);

        return builder.Build();
	}
}
