
using Org.Apache.Http.Authentication;

namespace BetBookGamingMobile.ViewModels;

public partial class MyBetsViewModel : AppBaseViewModel
{
    private readonly AuthenticationState _authState;

    public IEnumerable<SingleBetModel> bettorSingleBets;
    public ObservableCollection<SingleBetModel> BettorSingleBetsInProgress { get; } = new();
    public ObservableCollection<SingleBetModel> BettorSingleBetsWinners { get; } = new();
    public ObservableCollection<SingleBetModel> BettorSingleBetsPush { get; } = new();
    public ObservableCollection<SingleBetModel> BettorSingleBetsLosers { get; } = new();

    public IEnumerable<ParleyBetSlipModel> bettorParleyBets;
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
        if (IsBusy) return;
        var loggedInUser = _authState.CurrentAuthenticationState.LoggedInUser;

        if (string.IsNullOrEmpty(loggedInUser.UserId))
            return;

        IsBusy = true;
        try
        {
            await GetAndPopulateBettorSingleBetsAsync(loggedInUser);
            await GetAndPopulateBettorParleyBetsAsync(loggedInUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task GetAndPopulateBettorSingleBetsAsync(UserModel loggedInUser)
    {
        bettorSingleBets = await _apiService.GetAllBettorSingleBets(loggedInUser.UserId);

        BettorSingleBetsInProgress.AddRange(bettorSingleBets.Where(
            b => b.SingleBetStatus == SingleBetStatus.IN_PROGRESS), BettorSingleBetsInProgress.Any());

        BettorSingleBetsWinners.AddRange(bettorSingleBets.Where(
            b => b.SingleBetStatus == SingleBetStatus.WINNER), BettorSingleBetsWinners.Any());

        BettorSingleBetsLosers.AddRange(bettorSingleBets.Where(
            b => b.SingleBetStatus == SingleBetStatus.LOSER), BettorSingleBetsLosers.Any());

        BettorSingleBetsPush.AddRange(bettorSingleBets.Where(
            b => b.SingleBetStatus == SingleBetStatus.PUSH), BettorSingleBetsPush.Any());
    }

    private async Task GetAndPopulateBettorParleyBetsAsync(UserModel loggedInUser)
    {
        bettorParleyBets = await _apiService.GetAllBettorParleyBets(loggedInUser.UserId);

        BettorParleyBetsInProgress.AddRange(bettorParleyBets.Where(
            b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.IN_PROGRESS), BettorParleyBetsInProgress.Any());

        BettorParleyBetsWinners.AddRange(bettorParleyBets.Where(
            b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.WINNER), BettorParleyBetsWinners.Any());

        BettorParleyBetsLosers.AddRange(bettorParleyBets.Where(
            b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.LOSER), BettorParleyBetsLosers.Any());

        BettorParleyBetsPush.AddRange(bettorParleyBets.Where(
            b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.PUSH), BettorParleyBetsPush.Any());
    }
}
