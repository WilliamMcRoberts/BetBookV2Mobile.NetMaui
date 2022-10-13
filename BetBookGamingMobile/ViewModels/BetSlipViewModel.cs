
namespace BetBookGamingMobile.ViewModels;

public partial class BetSlipViewModel : BaseViewModel
{
    private readonly AuthenticationState _authenticationState;
    private readonly BetSlipState _betSlipState;

    public BetSlipViewModel(
        AuthenticationState authenticationState, BetSlipState betSlipState)
	{
        _authenticationState = authenticationState;
        _betSlipState = betSlipState;
    }

    [ObservableProperty]
    private string _parleyWagerAmount;

    [ObservableProperty]
    private string _singlesPayoutDisplay;

    [ObservableProperty]
    private string _parleyPayoutDisplay;

    [ObservableProperty]
    private BetSlipStateModel _betSlipStateModel;

    [RelayCommand]
    private async Task SubmitSinglesWagerAsync()
    {
        var loggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;

        if (string.IsNullOrEmpty(loggedInUser.UserId)) return;

        bool singlesBetSlipGood = await _betSlipState.OnSubmitBetsFromSinglesBetSlip(loggedInUser);

        await singlesBetSlipGood.ShowWagerConfirmationToastAsync("singles");

        BetSlipStateModel = singlesBetSlipGood ? _betSlipState.GetBetSlipState() : BetSlipStateModel;
    }

    [RelayCommand]
    private async Task SubmitParleyWagerAsync()
    {
        var loggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;

        if (string.IsNullOrEmpty(loggedInUser.UserId)) return;

        bool parleyBetSlipGood = decimal.TryParse(ParleyWagerAmount, out var parleyWager) && 
            await _betSlipState.OnSubmitBetsFromParleyBetSlip(loggedInUser, parleyWager);

        await parleyBetSlipGood.ShowWagerConfirmationToastAsync("parley");

        BetSlipStateModel = parleyBetSlipGood ? _betSlipState.GetBetSlipState() : BetSlipStateModel;

        ParleyWagerAmount = String.Empty;
    }

    [RelayCommand]
    private void RemoveBetFromPreBets(CreateBetModel createBet)
    {
        BetSlipStateModel = _betSlipState.RemoveBetFromPreBetsList(createBet);
        GetPayoutForTotalBetsSingles();
        GetPayoutForTotalBetsParley();
    }

    [RelayCommand]
    private void SetState()
    {
        BetSlipStateModel = _betSlipState.GetBetSlipState();
        GetPayoutForTotalBetsSingles();
        GetPayoutForTotalBetsParley();
    }
        
    [RelayCommand]
    private void GetPayoutForTotalBetsSingles() => 
        SinglesPayoutDisplay = "Total singles payout  " +
        $"  {_betSlipState.GetPayoutForTotalBetsSingles():C}";

    [RelayCommand]
    private void GetPayoutForTotalBetsParley() => 
        ParleyPayoutDisplay = decimal.TryParse(ParleyWagerAmount, out var parleyWager) || string.IsNullOrEmpty(ParleyWagerAmount) ? 
            "Total parley payout  " +
             $"  {_betSlipState.GetPayoutForTotalBetsParley(parleyWager):C}" : ParleyPayoutDisplay;

    
}
