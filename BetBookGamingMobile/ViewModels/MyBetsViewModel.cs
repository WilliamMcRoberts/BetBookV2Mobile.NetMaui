
namespace BetBookGamingMobile.ViewModels;

public partial class MyBetsViewModel : AppBaseViewModel
{
    private readonly AuthenticationState _authState;

    [ObservableProperty]
    private UserModel loggedInUser;

    private IEnumerable<SingleBetModel> bettorSingleBets;
    public ObservableCollection<SingleBetModel> BettorSingleBetsInProgress { get; } = new();
    public ObservableCollection<SingleBetModel> BettorSingleBetsWinners { get; } = new();
    public ObservableCollection<SingleBetModel> BettorSingleBetsPush { get; } = new();
    public ObservableCollection<SingleBetModel> BettorSingleBetsLosers { get; } = new();

    private IEnumerable<ParleyBetSlipModel> bettorParleyBets;
    public ObservableCollection<ParleyBetSlipModel> BettorParleyBetsInProgress { get; } = new();
    public ObservableCollection<ParleyBetSlipModel> BettorParleyBetsWinners { get; } = new();
    public ObservableCollection<ParleyBetSlipModel> BettorParleyBetsPush { get; } = new();
    public ObservableCollection<ParleyBetSlipModel> BettorParleyBetsLosers { get; } = new();

    public MyBetsViewModel(AuthenticationState authState, IApiService apiService) :base(apiService)
	{
        _authState = authState;
    }

    [RelayCommand]
    private async Task SetStateAsync()
    {
        if (IsBusy)
        {
            await Shell.Current.DisplayAlert(
                "App is busy", "Application is busy doing something else...please try again in a moment", "OK");
            return;
        }

        LoggedInUser = _authState.CurrentAuthenticationState.LoggedInUser;

        IsLoggedIn = LoggedInUser is not null && !string.IsNullOrEmpty(LoggedInUser.UserId);

        if (isNotLoggedIn)
        {
            await Shell.Current.DisplayAlert(
                "Not logged in", "You have to be logged in to view wager history", "OK");
            return;
        }

        IsBusy = true;

        try
        {
            await PopulateBettorSingleBetsAsync(LoggedInUser);
            await PopulateBettorParleyBetsAsync(LoggedInUser);
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
    }

    private async Task PopulateBettorSingleBetsAsync(UserModel loggedInUser)
    {
        bettorSingleBets = await _apiService.GetAllBettorSingleBets(loggedInUser.UserId);
        BettorSingleBetsInProgress.AddRange(bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.IN_PROGRESS), BettorSingleBetsInProgress.Any());
        BettorSingleBetsWinners.AddRange(bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.WINNER), BettorSingleBetsWinners.Any());
        BettorSingleBetsLosers.AddRange(bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.LOSER), BettorSingleBetsLosers.Any());
        BettorSingleBetsPush.AddRange(bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.PUSH), BettorSingleBetsPush.Any());
    }

    private async Task PopulateBettorParleyBetsAsync(UserModel loggedInUser)
    {
        bettorParleyBets = await _apiService.GetAllBettorParleyBets(loggedInUser.UserId);
        BettorParleyBetsInProgress.AddRange(bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.IN_PROGRESS), BettorParleyBetsInProgress.Any());
        BettorParleyBetsWinners.AddRange(bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.WINNER), BettorParleyBetsWinners.Any());
        BettorParleyBetsLosers.AddRange(bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.LOSER), BettorParleyBetsLosers.Any());
        BettorParleyBetsPush.AddRange(bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.PUSH), BettorParleyBetsPush.Any());
    }  
}
