
namespace BetBookGamingMobile.ViewModels;

[QueryProperty("GameDto", "GameDto")]
public partial class GameDetailsViewModel : AppBaseViewModel
{
    public event BetSelectedOrRemoved BetsChanged;

    public delegate void BetSelectedOrRemoved(List<CreateBetModel> betList, BetType betType);

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
        BetsChanged?.Invoke(_betSlipState.preBets, BetType.POINTSPREAD);
        await _betSlipState.preBets.Count.ShowBetNumberToastAsync();
    }
        
    [RelayCommand]
    private async Task SelectOrRemoveWagerForMoneylineAsync(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.MONEYLINE);
        BetsChanged?.Invoke(_betSlipState.preBets, BetType.MONEYLINE);
        await _betSlipState.preBets.Count.ShowBetNumberToastAsync();
    }
    
    [RelayCommand]
    private async Task SelectOrRemoveWagerForOverUnderAsync(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(
            string.Concat(winner, GameDto.ScoreID.ToString()), GameDto, BetType.OVERUNDER);
        BetsChanged?.Invoke(_betSlipState.preBets, BetType.OVERUNDER);
        await _betSlipState.preBets.Count.ShowBetNumberToastAsync();
    }

    [RelayCommand]
    private void SetState()
    {
        ButtonTextState = GameDto.GetButtonTextState();
        BetsChanged?.Invoke(_betSlipState.preBets, BetType.POINTSPREAD);
        BetsChanged?.Invoke(_betSlipState.preBets, BetType.MONEYLINE);
        BetsChanged?.Invoke(_betSlipState.preBets, BetType.OVERUNDER);
    }
}
