
namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : BasePage<GameDetailsViewModel>
{
    private readonly BetSlipState _betSlipState;

    public GameDetailsPage(GameDetailsViewModel viewModel, BetSlipState betSlipState) :base(viewModel)
    {
        InitializeComponent();
        _betSlipState = betSlipState;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        ViewModel.SetStateCommand.Execute(null);
    }

    async void ShowToast(object sender, EventArgs args)
    {
        var toast = _betSlipState.preBets.Count != 1 ? Toast.Make($"You have {_betSlipState.preBets.Count} bets in your bet slip", textSize: 15) 
            : Toast.Make($"You have 1 bet in your bet slip", textSize: 15);
        await toast.Show();
    }
}
