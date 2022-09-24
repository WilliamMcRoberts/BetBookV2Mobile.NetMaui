using BetBookGamingMobile.ViewModels;

namespace BetBookGamingMobile;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}

