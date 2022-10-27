

namespace BetBookGamingMobile.ViewModels;

public partial class BetSlipViewModel : BaseViewModel
{
    private readonly AuthenticationState _authenticationState;
    private readonly BetSlipState _betSlipState;
    public ObservableCollection<CreateBetModel> Bets { get; } = new();

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

        if (!singlesBetSlipGood)
        {
            if (_betSlipState.gameHasStarted)
            {
                await Shell.Current.DisplayAlert(
                "Wager not submitted", _betSlipState.startedGameDescription, "OK");
            }

            if (_betSlipState.betAmountForSinglesBad)
            {
                await Shell.Current.DisplayAlert(
                "Wager not submitted", "All single wagers must be more than $0.00", "OK");
            }

            return;
        }

        Bets.Clear();

        await "singles".ShowWagerConfirmationToastAsync();
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

        bool parleyBetSlipGood = false;

        IsBusy = true;

        try
        {
            parleyBetSlipGood = decimal.TryParse(ParleyWagerAmount, out var parleyWager) &&
                await _betSlipState.OnSubmitBetsFromParleyBetSlip(LoggedInUser, parleyWager);
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert(
                "Something went wrong", "Please try again in a moment", "OK");
        }
        finally
        {
            IsBusy = false;
        }

        if (!parleyBetSlipGood)
        {
            if (_betSlipState.gameHasStarted)
            {
                await Shell.Current.DisplayAlert(
                "Wager not submitted", _betSlipState.startedGameDescription, "OK");
            }

            if (_betSlipState.betAmountForParleyBad)
            {
                await Shell.Current.DisplayAlert(
                "Wager not submitted", "Parley wager must be more than $0.00", "OK");
            }

            return;
        }

        await "parley".ShowWagerConfirmationToastAsync();

        Bets.Clear();

        ParleyWagerAmount = String.Empty;
    }

    [RelayCommand]
    private void RemoveBetFromPreBets(CreateBetModel createBet)
    {
        if (_betSlipState.preBets.Contains(createBet) && Bets.Contains(createBet))
        {
            _betSlipState.preBets.Remove(createBet);
            Bets.Remove(createBet);
            _betSlipState.CheckForConflictingBets();
        }
        GetPayoutForTotalBetsSingles();
        GetPayoutForTotalBetsParley();
    }

    [RelayCommand]
    private void SetState()
    {
        Bets.AddRange(_betSlipState.preBets, Bets.Any());
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
