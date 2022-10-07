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

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

		if(!_viewModel.bettorSingleBets.Any() && !_viewModel.bettorParleyBets.Any())
			await _viewModel.SetStateCommand.ExecuteAsync(null);
    }
}