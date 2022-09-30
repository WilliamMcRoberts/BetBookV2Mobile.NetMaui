using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.ViewModels;
using MediatR;

namespace BetBookGamingMobile.Views;

public partial class BetSlipPage : ContentPage
{
    private readonly BetSlipViewModel _viewModel;
    private readonly IMediator _mediator;

    public BetSlipPage(BetSlipViewModel viewModel, IMediator mediator)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
        _mediator = mediator;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());
    }
}