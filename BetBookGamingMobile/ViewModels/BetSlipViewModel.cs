
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
    private UserModel loggedInUser;

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
        if (IsBusy) return;

        LoggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;

        if (string.IsNullOrEmpty(LoggedInUser.UserId)) return;

        bool singlesBetSlipGood = false;

        IsBusy = true;

        try
        {
            singlesBetSlipGood = await _betSlipState.OnSubmitBetsFromSinglesBetSlip(LoggedInUser);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }

        await singlesBetSlipGood.ShowWagerConfirmationToastAsync("singles");

        BetSlipStateModel = singlesBetSlipGood ? _betSlipState.GetBetSlipState() : BetSlipStateModel;
    }

    [RelayCommand]
    private async Task SubmitParleyWagerAsync()
    {
        if (IsBusy) return;

        LoggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;

        if (string.IsNullOrEmpty(LoggedInUser.UserId)) return;

        bool parleyBetSlipGood = false;

        IsBusy = true;

        try
        {
            parleyBetSlipGood = decimal.TryParse(ParleyWagerAmount, out var parleyWager) &&
                await _betSlipState.OnSubmitBetsFromParleyBetSlip(LoggedInUser, parleyWager);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }

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
