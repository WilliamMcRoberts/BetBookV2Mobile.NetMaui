using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.ViewModels;

namespace BetBookGamingMobile.Views;

public partial class BetSlipPage : ContentPage
{
    private readonly BetSlipViewModel _viewModel;
    private readonly BetSlipState _betSlip;

    public BetSlipPage(BetSlipViewModel viewModel, BetSlipState betSlip)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _viewModel = viewModel;
        _betSlip = betSlip;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.BetSlip = _betSlip.GetBetSlipState();
    }
}