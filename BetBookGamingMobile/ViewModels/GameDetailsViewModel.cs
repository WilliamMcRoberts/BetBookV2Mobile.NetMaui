
namespace BetBookGamingMobile.ViewModels;

[QueryProperty("GameDto", "GameDto")]
public partial class GameDetailsViewModel : BaseViewModel
{
    private readonly BetSlipState _betSlipState;

    [ObservableProperty]
    GameDto gameDto;

    [ObservableProperty]
    ButtonColorStateModel buttonColorState;

    [ObservableProperty]
    ButtonTextStateModel buttonTextState;

    [ObservableProperty]
    BetSlipStateModel betSlipStateModel;

    public GameDetailsViewModel(BetSlipState betSlipState)
    {
        _betSlipState = betSlipState;
    }

    [RelayCommand]
    private void SelectOrRemoveWagerForPointSpread(string winner) => 
        ButtonColorState =
            _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.POINTSPREAD);

    [RelayCommand]
    private void SelectOrRemoveWagerForMoneyline(string winner) => 
        ButtonColorState =
            _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.MONEYLINE);

    [RelayCommand]
    private void SelectOrRemoveWagerForOverUnder(string winner) => 
        ButtonColorState = _betSlipState.SelectOrRemoveWinnerAndGameForBet(string.Concat(
            winner, GameDto.ScoreID.ToString()), GameDto, BetType.OVERUNDER);

    [RelayCommand]
    private void SetState() => 
        (BetSlipStateModel, ButtonColorState, ButtonTextState) = _betSlipState.GetAllStates(GameDto);
}
