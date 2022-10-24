

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
        if (IsBusy)
        {
            await Shell.Current.DisplayAlert(
                "App is busy", "Application is busy doing something else...please try again in a moment", "OK");
            return;
        }

        LoggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;

        IsLoggedIn = LoggedInUser is not null && !string.IsNullOrEmpty(LoggedInUser.UserId);

        if (isNotLoggedIn)
        {
            await Shell.Current.DisplayAlert(
                "Not logged in", "You have to be logged in to place a wager", "OK");
            return;
        }

        bool singlesBetSlipGood = false;

        IsBusy = true;

        try
        {
            singlesBetSlipGood = await _betSlipState.OnSubmitBetsFromSinglesBetSlip(LoggedInUser);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Something went wrong", ex.Message, "OK");
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
        if (IsBusy)
        {
            await Shell.Current.DisplayAlert(
                "App is busy", "Application is busy doing something else...please try again in a moment", "OK");
            return;
        }

        if (_betSlipState.conflictingBetsForParley)
        {
            await Shell.Current.DisplayAlert(
                "Conflicting Bets", "You have conflicting bets in your bet slip...parley wager not possible", "OK");
        }

        LoggedInUser = _authenticationState.CurrentAuthenticationState.LoggedInUser;

        IsLoggedIn = LoggedInUser is not null && !string.IsNullOrEmpty(LoggedInUser.UserId);

        if (isNotLoggedIn)
        {
            await Shell.Current.DisplayAlert(
                "Not logged in", "You have to be logged in to place a wager", "OK");
            return;
        }

        bool parleyBetSlipGood = false;

        IsBusy = true;

        try
        {
            parleyBetSlipGood = decimal.TryParse(ParleyWagerAmount, out var parleyWager) &&
                await _betSlipState.OnSubmitBetsFromParleyBetSlip(LoggedInUser, parleyWager);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Something went wrong", ex.Message, "OK");
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
