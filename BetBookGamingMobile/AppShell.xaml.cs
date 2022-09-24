using BetBookGamingMobile.Views;

namespace BetBookGamingMobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(GameDetailsPage), typeof(GameDetailsPage));
    }
}
