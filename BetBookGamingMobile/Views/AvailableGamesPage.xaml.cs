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

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.Games.Count > 0)
            return;

        _viewModel.Season = DateTime.Now.CalculateSeason();
        _viewModel.WeekNumber = _viewModel.Season.CalculateWeek(DateTime.Now);

        await _viewModel.GetGamesCommand.ExecuteAsync(null);

        _viewModel.Title = _viewModel.GetTitle();
    }
}