using BetBookGamingMobile.ViewModels;

namespace BetBookGamingMobile.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}