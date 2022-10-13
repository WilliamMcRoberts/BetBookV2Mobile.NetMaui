
namespace BetBookGamingMobile.ViewModels;

public partial class BetSlipViewModel : BaseViewModel
{
    private readonly AuthenticationState _authenticationState;
    private readonly BetSlipState _betSlipState;
    public bool parleyBetSlipGood = false;
    public bool singlesBetSlipGood = false;

    public BetSlipViewModel(
        AuthenticationState authenticationState, BetSlipState betSlipState)
	{
        _authenticationState = authenticationState;
        _betSlipState = betSlipState;
    }

#nullable enable
    [ObservableProperty]
    private string _parleyWagerAmount;
#nullable disable

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

        if (string.IsNullOrEmpty(loggedInUser.UserId)) 
            return;

        singlesBetSlipGood =
            await _betSlipState.OnSubmitBetsFromSinglesBetSlip(loggedInUser);

        if (singlesBetSlipGood)
            BetSlipStateModel = _betSlipState.GetBetSlipState();
    }

    [RelayCommand]
    private async Task SubmitParleyWagerAsync()
    {
        var loggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;

        if (string.IsNullOrEmpty(loggedInUser.UserId)) 
            return;

        if(decimal.TryParse(ParleyWagerAmount, out var parleyWager))
            parleyBetSlipGood = await _betSlipState.OnSubmitBetsFromParleyBetSlip(loggedInUser, parleyWager);

        if (parleyBetSlipGood)
            BetSlipStateModel = _betSlipState.GetBetSlipState();

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
        SinglesPayoutDisplay = $"Total singles payout    $0.00";
        ParleyPayoutDisplay = $"Total parley payout    $0.00";
    }
        
    [RelayCommand]
    private void GetPayoutForTotalBetsSingles() =>
        SinglesPayoutDisplay = "Total singles payout  " 
            +
        $"  ${_betSlipState.GetPayoutForTotalBetsSingles():#,##0.00}";

    [RelayCommand]
    private void GetPayoutForTotalBetsParley()
    {
        if(decimal.TryParse(ParleyWagerAmount, out var parleyWager) || string.IsNullOrEmpty(ParleyWagerAmount))
            ParleyPayoutDisplay = "Total parley payout  "
                +
            $"  ${_betSlipState.GetPayoutForTotalBetsParley(parleyWager):#,##0.00}";
    }
        
        
}
