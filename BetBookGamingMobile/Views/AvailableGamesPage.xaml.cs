using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.ViewModels;

namespace BetBookGamingMobile.Views;

public partial class AvailableGamesPage : ContentPage
{
    private readonly AvailableGamesViewModel _viewModel;

    public AvailableGamesPage(AvailableGamesViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (_viewModel.Games.Count > 0)
            return;

        _viewModel.SetStateCommand.Execute(null);
        await _viewModel.GetGamesCommand.ExecuteAsync(null);
    }
}