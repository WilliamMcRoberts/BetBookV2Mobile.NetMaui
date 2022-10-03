using BetBookGamingMobile.Queries;
using BetBookGamingMobile.ViewModels;

namespace BetBookGamingMobile.Views;

public partial class MyBetsPage : ContentPage
{
	private readonly MyBetsViewModel _viewModel;

	public MyBetsPage(MyBetsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // TODO - Call Get Bettor Single Bets and Get Bettor Parley Bets Methods
    }
}