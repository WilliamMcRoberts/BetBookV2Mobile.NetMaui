
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
    decimal parleyWagerAmount;

    [ObservableProperty]
    BetSlipStateModel betSlipState;

    [RelayCommand]
    private async Task SubmitSinglesWagerAsync()
    {
        var loggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;
            
        if (string.IsNullOrEmpty(loggedInUser.UserId)) return;

        bool singlesBetSlipGood =
            await _betSlipState.OnSubmitBetsFromSinglesBetSlip(loggedInUser);

        if (singlesBetSlipGood) 
            BetSlipState = _betSlipState.GetBetSlipState();
    }

    [RelayCommand]
    private async Task SubmitParleyWagerAsync()
    {
        var loggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;

        if (string.IsNullOrEmpty(loggedInUser.UserId)) return;

        bool parleyBetSlipGood = 
            await _betSlipState.OnSubmitBetsFromParleyBetSlip(loggedInUser, parleyWagerAmount);

        if (parleyBetSlipGood)
            BetSlipState = _betSlipState.GetBetSlipState();

        ParleyWagerAmount = 0;
    }

    [RelayCommand]
    private void RemoveBetFromPreBets(CreateBetModel createBet) =>
        BetSlipState = _betSlipState.RemoveBetFromPreBetsList(createBet);

    [RelayCommand]
    private void SetState() =>
        BetSlipState = _betSlipState.GetBetSlipState();
}
