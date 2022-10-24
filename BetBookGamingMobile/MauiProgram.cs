
namespace BetBookGamingMobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton(Connectivity.Current);

        /**********************    Authentication      ********************************/

        builder.Services.AddSingleton<IAuthService, AuthService>();

        /**********************    Services      **************************************/

        builder.Services.AddSingleton<IApiService, ApiService>();
       
        /**********************   State   *****************************************/

        builder.Services.AddScoped<BetSlipState>();
        builder.Services.AddScoped<AuthenticationState>();

        /***********************   View Models  ***************************************/

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<GameDetailsViewModel>();
        builder.Services.AddTransient<BetSlipViewModel>();
        builder.Services.AddTransient<MyBetsViewModel>();
        builder.Services.AddTransient<AvailableGamesViewModel>();

        /***************************   Views    ***************************************/

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<GameDetailsPage>();
        builder.Services.AddTransient<BetSlipPage>();
        builder.Services.AddTransient<MyBetsPage>();
        builder.Services.AddTransient<AvailableGamesPage>();

        return builder.Build();
    }

}



