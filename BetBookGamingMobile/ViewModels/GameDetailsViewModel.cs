

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BetBookGamingMobile.ViewModels;

[QueryProperty("GameDto", "GameDto")]
public partial class GameDetailsViewModel : BaseViewModel
{
    private readonly BetSlip _betSlip;
    private readonly IUserService _userService;
    
    [ObservableProperty]
    GameDto gameDto;

    [ObservableProperty]
    ButtonColorState buttonColorState;

    [ObservableProperty]
    ButtonTextState buttonTextState;

    [ObservableProperty]
    BetSlipState betSlipState;

    public GameDetailsViewModel(BetSlip betSlip, IUserService userService)
    {
        _betSlip = betSlip;
        _userService = userService;
    }    

    [RelayCommand]
    private void AddOrRemoveWagerForPointSpread(string winner)
    {
        _betSlip.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.POINTSPREAD);
        (BetSlipState, ButtonColorState) = GetButtonColorAndBetSlipStates();
    }

    [RelayCommand]
    private void AddOrRemoveWagerForMoneyline(string winner)
    {
        _betSlip.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.MONEYLINE);
        (BetSlipState, ButtonColorState) = GetButtonColorAndBetSlipStates();
    }

    [RelayCommand]
    private void AddOrRemoveWagerForOverUnder(string overUnder)
    {
        _betSlip.SelectOrRemoveWinnerAndGameForBet(string.Concat(overUnder, GameDto.ScoreID.ToString()), GameDto, BetType.OVERUNDER);
        (BetSlipState, ButtonColorState) = GetButtonColorAndBetSlipStates();
    }

    public (BetSlipState, ButtonColorState) GetButtonColorAndBetSlipStates() =>
        (_betSlip.GetBetSlipState(), _betSlip.GetButtonColorState(GameDto));

    public ButtonTextState GetButtonTextState() =>
        new ButtonTextState
        {
            ApText = $"{GameDto.AwayTeam} {GameDto.AwayTeamPointSpreadForDisplay} | {GameDto.PointSpreadAwayTeamMoneyLine}",
            HpText = $"{GameDto.HomeTeam} {GameDto.HomeTeamPointSpreadForDisplay} | {GameDto.PointSpreadHomeTeamMoneyLine}",
            AmText = $"{GameDto.AwayTeamMoneyLine}",
            HmText = $"{GameDto.HomeTeamMoneyLine}",
            OText = $"Over {GameDto.OverUnder} | {GameDto.OverPayout}",
            UText = $"Over {GameDto.OverUnder} | {GameDto.UnderPayout}"

        };

    [RelayCommand]
    private async Task GoToCurrentBetSlipAsync() => 
        await Shell.Current.GoToAsync(nameof(BetSlipPage), true);

    [RelayCommand]
    private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
}
