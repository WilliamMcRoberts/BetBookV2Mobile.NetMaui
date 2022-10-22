
namespace BetBookGamingMobile.ViewModels;

[QueryProperty("GameDto", "GameDto")]
public partial class GameDetailsViewModel : AppBaseViewModel
{
    public event PointSpreadBetSelectedOrRemoved PointSpreadBetsChanged;
    public delegate void PointSpreadBetSelectedOrRemoved(List<CreateBetModel> betList);
    public event MoneylineBetSelectedOrRemoved MoneylineBetsChanged;
    public delegate void MoneylineBetSelectedOrRemoved(List<CreateBetModel> betList);
    public event OverUnderBetSelectedOrRemoved OverUnderBetsChanged;
    public delegate void OverUnderBetSelectedOrRemoved(List<CreateBetModel> betList);
    private readonly BetSlipState _betSlipState;

    [ObservableProperty]
    private GameDto gameDto;

    [ObservableProperty]
    private ButtonTextStateModel buttonTextState;

    public GameDetailsViewModel(BetSlipState betSlipState, IApiService apiService) :base(apiService)
    {
        _betSlipState = betSlipState;
    }

    [RelayCommand]
    private async Task SelectOrRemoveWagerForPointSpreadAsync(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.POINTSPREAD);
        PointSpreadBetsChanged?.Invoke(_betSlipState.preBets);
        await _betSlipState.preBets.Count.ShowBetNumberToastAsync();
    }
        

    [RelayCommand]
    private async Task SelectOrRemoveWagerForMoneylineAsync(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.MONEYLINE);
        MoneylineBetsChanged?.Invoke(_betSlipState.preBets);
        await _betSlipState.preBets.Count.ShowBetNumberToastAsync();
    }
    

    [RelayCommand]
    private async Task SelectOrRemoveWagerForOverUnderAsync(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(
            string.Concat(winner, GameDto.ScoreID.ToString()), GameDto, BetType.OVERUNDER);
        OverUnderBetsChanged?.Invoke(_betSlipState.preBets);
        await _betSlipState.preBets.Count.ShowBetNumberToastAsync();
    }

    [RelayCommand]
    private void SetState()
    {
        ButtonTextState = GameDto.GetButtonTextState();
        PointSpreadBetsChanged?.Invoke(_betSlipState.preBets);
        MoneylineBetsChanged?.Invoke(_betSlipState.preBets);
        OverUnderBetsChanged?.Invoke(_betSlipState.preBets);
    }
}
