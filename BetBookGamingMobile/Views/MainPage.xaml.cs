using BetBookGamingMobile.Dto;
using BetBookGamingMobile.ViewModels;

namespace BetBookGamingMobile;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if(!_viewModel.gamesLoaded)
            await _viewModel.GetGamesAsync();
    }
}

