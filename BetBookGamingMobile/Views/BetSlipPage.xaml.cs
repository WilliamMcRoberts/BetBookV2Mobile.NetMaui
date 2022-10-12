
namespace BetBookGamingMobile.Views;

public partial class BetSlipPage : BasePage<BetSlipViewModel>
{
    
    public BetSlipPage(BetSlipViewModel viewModel) :base(viewModel)
	{
		InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        ViewModel.SetStateCommand.Execute(null);
    }

    private void SingleBetAmountEntry_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.GetPayoutForTotalBetsSinglesCommand.Execute(null);

    private void ParleyBetAmountEntry_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.GetPayoutForTotalBetsParleyCommand.Execute(null);

    private void SingleBetAmountEntry_Unfocused(object sender, FocusEventArgs e) =>
        ViewModel.SinglesPayoutDisplay = ViewModel.BetSlipStateModel.BetsInBetSlip.Where(bet => bet.BetAmount != 0).Any()
            ? $"Total singles payout    $0" : ViewModel.SinglesPayoutDisplay;

    private void ParleyBetAmountEntry_Unfocused(object sender, FocusEventArgs e) =>
        ViewModel.ParleyPayoutDisplay = ViewModel.BetSlipStateModel.BetsInBetSlip.Where(bet => bet.BetAmount != 0).Any()
            ? $"Total parley payout    $0" : ViewModel.ParleyPayoutDisplay;
}