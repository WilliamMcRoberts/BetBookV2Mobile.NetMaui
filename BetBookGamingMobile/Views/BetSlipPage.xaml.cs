
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

    private async void SubmitSinglesWagerAndShowToast(object sender, EventArgs args)
    {
        await ViewModel.SubmitSinglesWagerCommand.ExecuteAsync(null);
        if (ViewModel.singlesBetSlipGood)
        {
            var toast = Toast.Make($"Your singles wager was submitted!", textSize: 18);
            await toast.Show();
        }
    }

    private async void SubmitParleyWagerAndShowToast(object sender, EventArgs args)
    {
        await ViewModel.SubmitParleyWagerCommand.ExecuteAsync(null);
        if (ViewModel.parleyBetSlipGood)
        {
            var toast = Toast.Make($"Your parley wager was submitted!", textSize: 18);
            await toast.Show();
        }
    }
}