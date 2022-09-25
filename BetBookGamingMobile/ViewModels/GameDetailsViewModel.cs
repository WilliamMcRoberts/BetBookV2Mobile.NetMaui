

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BetBookGamingMobile.ViewModels;

[QueryProperty("GameDto", "GameDto")]
[QueryProperty("ButtonColorStateModel", "ButtonColorStateModel")]
[QueryProperty("BetSlipState", "BetSlipState")]
public partial class GameDetailsViewModel : BaseViewModel
{
    private readonly BetSlipState _betSlipState;
    private readonly IUserService _userService;
    

    [ObservableProperty]
    GameDto gameDto;

    [ObservableProperty]
    ButtonColorStateModel buttonColorStateModel;

    [ObservableProperty]
    BetSlipStateModel betSlipState;

    public GameDetailsViewModel(BetSlipState betSlipState, IUserService userService)
    {
        _betSlipState = betSlipState;
        _userService = userService;
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void AddWagerForPointSpread(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.POINTSPREAD);
        BetSlipState = _betSlipState.GetBetSlipState();
        ButtonColorStateModel = _betSlipState.GetButtonColorState(GameDto);
    }

    [RelayCommand]
    private void AddWagerForMoneyline(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.MONEYLINE);
        BetSlipState = _betSlipState.GetBetSlipState();
        ButtonColorStateModel = _betSlipState.GetButtonColorState(GameDto);
    }

    [RelayCommand]
    private void AddWagerForOverUnder(string overUnder)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(string.Concat(overUnder, GameDto.ScoreID.ToString()), GameDto, BetType.OVERUNDER);
        BetSlipState = _betSlipState.GetBetSlipState();
        ButtonColorStateModel = _betSlipState.GetButtonColorState(GameDto);
    }

    [RelayCommand]
    private async Task SubmitSinglesWager()
    {
        UserModel user = await _userService.GetUserByUserId("632395fdc17912bd030e4162");

        await _betSlipState.OnSubmitBetsFromSinglesBetSlip(user);
    }

    [RelayCommand]
    private async Task SubmitParleyWager()
    {
        UserModel user = await _userService.GetUserByUserId("632395fdc17912bd030e4162");

        await _betSlipState.OnSubmitBetsFromParleyBetSlip(user);
    }
}
