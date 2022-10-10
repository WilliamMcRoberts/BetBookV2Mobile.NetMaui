
namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : BasePage<GameDetailsViewModel>
{
    public GameDetailsPage(GameDetailsViewModel viewModel) :base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        ViewModel.SetStateCommand.Execute(null);
    }

    async void ShowToast(object sender, EventArgs args)
    {
        var toast = ViewModel.BetSlipStateModel.BetsInBetSlip.Count != 1 ? Toast.Make($"You have {ViewModel.BetSlipStateModel.BetsInBetSlip.Count} bets in your bet slip", textSize: 15) 
            : Toast.Make($"You have 1 bet in your bet slip", textSize: 15);
        await toast.Show();
    }
}
