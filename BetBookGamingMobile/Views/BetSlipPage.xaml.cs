using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.ViewModels;
using MediatR;

namespace BetBookGamingMobile.Views;

public partial class BetSlipPage : ContentPage
{
    private readonly BetSlipViewModel _viewModel;

    public BetSlipPage(BetSlipViewModel viewModel, IMediator mediator, AuthState authState)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.GetBetSlipStateCommand.ExecuteAsync(null);
    }
}