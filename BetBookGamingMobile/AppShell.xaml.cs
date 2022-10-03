using BetBookGamingMobile.Views;

namespace BetBookGamingMobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(MyBetsPage), typeof(MyBetsPage));
        Routing.RegisterRoute(nameof(AvailableGamesPage), typeof(AvailableGamesPage));
        Routing.RegisterRoute(nameof(GameDetailsPage), typeof(GameDetailsPage));
        Routing.RegisterRoute(nameof(BetSlipPage), typeof(BetSlipPage));
    }
}
