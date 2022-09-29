using BetBookGamingMobile.Views;

namespace BetBookGamingMobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(GameDetailsPage), typeof(GameDetailsPage));
        Routing.RegisterRoute(nameof(BetSlipPage), typeof(BetSlipPage));
    }
}
