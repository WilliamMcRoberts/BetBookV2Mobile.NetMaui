
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
        var authState = _authenticationState.GetCurrentAuthenticationState();

        LoggedInUser = authState.LoggedInUser;        
            
        if (string.IsNullOrEmpty(LoggedInUser.UserId)) return;

        bool singlesBetSlipGood =
            await _betSlipState.OnSubmitBetsFromSinglesBetSlip(LoggedInUser);

        if (singlesBetSlipGood) 
            BetSlipState = _betSlipState.GetBetSlipState();
    }

    [RelayCommand]
    private async Task SubmitParleyWagerAsync()
    {
        var authState = _authenticationState.GetCurrentAuthenticationState();

        LoggedInUser = authState.LoggedInUser;

        if (string.IsNullOrEmpty(LoggedInUser.UserId)) return;

        bool parleyBetSlipGood = 
            await _betSlipState.OnSubmitBetsFromParleyBetSlip(LoggedInUser, parleyWagerAmount);

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
