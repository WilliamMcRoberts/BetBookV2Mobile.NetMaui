
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
}