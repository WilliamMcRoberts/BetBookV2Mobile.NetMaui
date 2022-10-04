using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.ViewModels;
using MediatR;

namespace BetBookGamingMobile.Views;

public partial class BetSlipPage : ContentPage
{
    private readonly BetSlipViewModel _viewModel;

    public BetSlipPage(BetSlipViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await _viewModel.SetStateCommand.ExecuteAsync(null);
    }
}