
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.ViewModels;
using MediatR;

namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : ContentPage
{
    private readonly GameDetailsViewModel _viewModel;

    public GameDetailsPage(GameDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.SetStateCommand.ExecuteAsync(null);
    }
}