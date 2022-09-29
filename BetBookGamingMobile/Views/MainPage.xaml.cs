using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.ViewModels;

namespace BetBookGamingMobile;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if(_viewModel.Games.Count < 1)
        {
            _viewModel.Season = DateTime.Now.CalculateSeason();
            _viewModel.WeekNumber = _viewModel.Season.CalculateWeek(DateTime.Now);
            _viewModel.Title = $"Games {_viewModel.Season} Season Week {_viewModel.WeekNumber}";
            await _viewModel.GetGamesCommand.ExecuteAsync(null);
        }
    }
}

