using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.ViewModels;
using BetBookGamingMobile.Views;
using Microsoft.Extensions.Configuration;
using MediatR;
using BetBookGamingMobile.Auth;
using static Android.Telephony.CarrierConfigManager;

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

        /*********************** Http Client Factory **************************/
        
        builder.Services.AddHttpClient("sportsdata", client =>
        {
            client.BaseAddress = new Uri("https://api.sportsdata.io/v3/nfl/");
        });

        builder.Services.AddHttpClient("vortex", client =>
        {
            client.BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? 
                                    new Uri("https://user9f9bd262219b696.app.vtxhub.com/") 
                                    : new Uri("https://localhost:7184/");
        });
        
        builder.Services.AddSingleton<IGameService, GameService>();
        builder.Services.AddTransient<ISingleBetService, SingleBetService>();
        builder.Services.AddTransient<IParleyBetSlipService, ParleyBetSlipService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IAuthService, AuthService>();

        builder.Services.AddScoped<BetSlip>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<GameDetailsViewModel>();
        builder.Services.AddTransient<BetSlipViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<GameDetailsPage>();
        builder.Services.AddTransient<BetSlipPage>();
        builder.Services.AddTransient<ProfilePage>();

        builder.Services.AddMediatR(typeof(MediatREntryPoint).Assembly);
        builder.Services.AddSingleton(Connectivity.Current);

        builder.Configuration.AddUserSecrets("e7d4ad5e-3fed-44c5-846f-c09a4742a4cd");

        return builder.Build();
    }

}



