
namespace BetBookGamingMobile.ViewModels;

public partial class MyBetsViewModel : BaseViewModel
{
    private readonly ISingleBetService _singleBetService;
    private readonly IParleyBetSlipService _parleyBetSlipService;
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

    public MyBetsViewModel(ISingleBetService singleBetService, IParleyBetSlipService parleyBetSlipService, AuthenticationState authState)
	{
        _singleBetService = singleBetService;
        _parleyBetSlipService = parleyBetSlipService;
        _authState = authState;
    }

    [RelayCommand]
    private async Task SetStateAsync()
    {
        var authState = _authState.GetCurrentAuthenticationState();

        LoggedInUser = authState.LoggedInUser;

        if (string.IsNullOrEmpty(LoggedInUser.UserId))
            return;

        await GetAndPopulateBettorSingleBetsAsync();
        await GetAndPopulateBettorParleyBetsAsync();
    }

    private async Task GetAndPopulateBettorSingleBetsAsync()
    {
        bettorSingleBets = await _singleBetService.GetAllBettorSingleBets(LoggedInUser.UserId);

        BettorSingleBetsInProgress.AddRange(bettorSingleBets.Where(
            b => b.SingleBetStatus == SingleBetStatus.IN_PROGRESS), BettorSingleBetsInProgress.Any());

        BettorSingleBetsWinners.AddRange(bettorSingleBets.Where(
            b => b.SingleBetStatus == SingleBetStatus.WINNER), BettorSingleBetsWinners.Any());

        BettorSingleBetsLosers.AddRange(bettorSingleBets.Where(
            b => b.SingleBetStatus == SingleBetStatus.LOSER), BettorSingleBetsLosers.Any());

        BettorSingleBetsPush.AddRange(bettorSingleBets.Where(
            b => b.SingleBetStatus == SingleBetStatus.PUSH), BettorSingleBetsPush.Any());
    }

    private async Task GetAndPopulateBettorParleyBetsAsync()
    {
        bettorParleyBets = await _parleyBetSlipService.GetAllBettorParleyBets(LoggedInUser.UserId);

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
