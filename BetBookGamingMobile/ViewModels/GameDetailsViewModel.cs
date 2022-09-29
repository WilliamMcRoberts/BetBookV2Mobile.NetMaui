

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BetBookGamingMobile.ViewModels;

[QueryProperty("GameDto", "GameDto")]
public partial class GameDetailsViewModel : BaseViewModel
{
    private readonly BetSlipState _betSlipState;
    private readonly IUserService _userService;
    
    [ObservableProperty]
    GameDto gameDto;

    [ObservableProperty]
    ButtonColorStateModel buttonColorState;

    [ObservableProperty]
    ButtonTextStateModel buttonTextState;

    [ObservableProperty]
    BetSlipStateModel betSlip;

    public GameDetailsViewModel(BetSlipState betSlipState, IUserService userService)
    {
        _betSlipState = betSlipState;
        _userService = userService;
    }    

    [RelayCommand]
    private void AddOrRemoveWagerForPointSpread(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.POINTSPREAD);
        (BetSlip, ButtonColorState) = GetButtonColorAndBetSlipStates();
    }

    [RelayCommand]
    private void AddOrRemoveWagerForMoneyline(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.MONEYLINE);
        (BetSlip, ButtonColorState) = GetButtonColorAndBetSlipStates();
    }

    [RelayCommand]
    private void AddOrRemoveWagerForOverUnder(string overUnder)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(string.Concat(overUnder, GameDto.ScoreID.ToString()), GameDto, BetType.OVERUNDER);
        (BetSlip, ButtonColorState) = GetButtonColorAndBetSlipStates();
    }

    public (BetSlipStateModel, ButtonColorStateModel) GetButtonColorAndBetSlipStates() =>
        (_betSlipState.GetBetSlipState(), _betSlipState.GetButtonColorState(GameDto));

    public ButtonTextStateModel GetButtonTextState() =>
        new ButtonTextStateModel
        {
            ApText = $"{GameDto.AwayTeam} {GameDto.AwayTeamPointSpreadForDisplay} Payout: {GameDto.PointSpreadAwayTeamMoneyLine}",
            HpText = $"{GameDto.HomeTeam} {GameDto.HomeTeamPointSpreadForDisplay} Payout: {GameDto.PointSpreadHomeTeamMoneyLine}",
            AmText = $"Payout: {GameDto.AwayTeamMoneyLine}",
            HmText = $"Payout: {GameDto.HomeTeamMoneyLine}",
            OText = $"Over {GameDto.OverUnder} Payout: {GameDto.OverPayout}",
            UText = $"Over {GameDto.OverUnder} Payout: {GameDto.UnderPayout}"

        };

    [RelayCommand]
    private async Task GoToCurrentBetSlipAsync() => 
        await Shell.Current.GoToAsync(nameof(BetSlipPage), true);

    [RelayCommand]
    private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
}
