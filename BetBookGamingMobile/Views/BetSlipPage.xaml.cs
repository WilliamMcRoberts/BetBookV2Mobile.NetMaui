using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.ViewModels;

namespace BetBookGamingMobile.Views;

public partial class BetSlipPage : ContentPage
{
    private readonly BetSlipViewModel _viewModel;
    private readonly BetSlip _betSlip;

    public BetSlipPage(BetSlipViewModel viewModel, BetSlip betSlip)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
        _betSlip = betSlip;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.BetSlipState = _betSlip.GetBetSlipState();
    }
}