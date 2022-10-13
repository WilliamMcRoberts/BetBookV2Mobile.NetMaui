
namespace BetBookGamingMobile.ViewModels;

[QueryProperty("GameDto", "GameDto")]
public partial class GameDetailsViewModel : AppBaseViewModel
{
     private readonly BetSlipState _betSlipState;

    [ObservableProperty]
    private GameDto gameDto;

    [ObservableProperty]
    private ButtonColorStateModel buttonColorState;

    [ObservableProperty]
    private ButtonTextStateModel buttonTextState;

    [ObservableProperty]
    private BetSlipStateModel betSlipStateModel;

    public GameDetailsViewModel(BetSlipState betSlipState, IApiService apiService) :base(apiService)
    {
        _betSlipState = betSlipState;
    }

    [RelayCommand]
    private async Task SelectOrRemoveWagerForPointSpreadAsync(string winner)
    {
        ButtonColorState =
            _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.POINTSPREAD);
        await _betSlipState.preBets.Count.ShowBetNumberToastAsync();
    }
        

    [RelayCommand]
    private async Task SelectOrRemoveWagerForMoneylineAsync(string winner)
    {
        ButtonColorState =
            _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.MONEYLINE);
        await _betSlipState.preBets.Count.ShowBetNumberToastAsync();
    }
    

    [RelayCommand]
    private async Task SelectOrRemoveWagerForOverUnderAsync(string winner)
    {
        ButtonColorState = _betSlipState.SelectOrRemoveWinnerAndGameForBet(
            string.Concat(winner, GameDto.ScoreID.ToString()), GameDto, BetType.OVERUNDER);
        await _betSlipState.preBets.Count.ShowBetNumberToastAsync();
    }

    [RelayCommand]
    private void SetState() => 
        (ButtonColorState, ButtonTextState) = _betSlipState.GetAllStates(GameDto);
}
