
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

        builder.Services.AddSingleton(Connectivity.Current);

        /**********************    Authentication      ********************************/

        builder.Services.AddSingleton<IAuthService, AuthService>();

        /**********************    Services      **************************************/

        builder.Services.AddSingleton<IGameService, GameService>();
        builder.Services.AddTransient<ISingleBetService, SingleBetService>();
        builder.Services.AddTransient<IParleyBetSlipService, ParleyBetSlipService>();
        builder.Services.AddTransient<IUserService, UserService>();        

        /**********************   Global State   *****************************************/

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

        builder.Configuration.AddUserSecrets("e7d4ad5e-3fed-44c5-846f-c09a4742a4cd");

        return builder.Build();
    }

}



