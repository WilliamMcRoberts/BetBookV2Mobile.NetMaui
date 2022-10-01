
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.ViewModels;
using MediatR;

namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : ContentPage
{
    private readonly GameDetailsViewModel _viewModel;
    private readonly IMediator _mediator;

    public GameDetailsPage(GameDetailsViewModel viewModel, IMediator mediator)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _mediator = mediator;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        (_viewModel.BetSlipState, _viewModel.ButtonColorState, _viewModel.ButtonTextState) =
            await _mediator.Send(new GetAllStatesQuery(_viewModel.GameDto));
    }
}