
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

#nullable enable
    [ObservableProperty]
    public decimal? parleyWagerAmount;
#nullable disable

    [ObservableProperty]
    string singlesPayoutDisplay;

    [ObservableProperty]
    string parleyPayoutDisplay;

    [ObservableProperty]
    BetSlipStateModel betSlipStateModel;

    [RelayCommand]
    private async Task SubmitSinglesWagerAsync()
    {
        var loggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;
            
        if (string.IsNullOrEmpty(loggedInUser.UserId)) return;

        bool singlesBetSlipGood =
            await _betSlipState.OnSubmitBetsFromSinglesBetSlip(loggedInUser);

        if (singlesBetSlipGood) 
            BetSlipStateModel = _betSlipState.GetBetSlipState();
    }

    [RelayCommand]
    private async Task SubmitParleyWagerAsync()
    {
        var loggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;

        if (string.IsNullOrEmpty(loggedInUser.UserId)) return;

        bool parleyBetSlipGood = 
            await _betSlipState.OnSubmitBetsFromParleyBetSlip(loggedInUser, (decimal)parleyWagerAmount);

        if (parleyBetSlipGood)
            BetSlipStateModel = _betSlipState.GetBetSlipState();

        ParleyWagerAmount = 0;
    }

    [RelayCommand]
    private void RemoveBetFromPreBets(CreateBetModel createBet) =>
        BetSlipStateModel = _betSlipState.RemoveBetFromPreBetsList(createBet);

    [RelayCommand]
    private void SetState()
    {
        BetSlipStateModel = _betSlipState.GetBetSlipState();
        SinglesPayoutDisplay = $"Total singles payout    $0";
        ParleyPayoutDisplay = $"Total parley payout    $0";
    }
        
    [RelayCommand]
    private void GetPayoutForTotalBetsSingles() =>
        SinglesPayoutDisplay = $"Total singles payout  " 
            +
            $"  ${_betSlipState.GetPayoutForTotalBetsSingles():#,##0.00}";

    [RelayCommand]
    private void GetPayoutForTotalBetsParley() =>
        ParleyPayoutDisplay = $"Total parley payout  " 
            +
           $"  ${_betSlipState.GetPayoutForTotalBetsParley((decimal)parleyWagerAmount):#,##0.00}";
}
