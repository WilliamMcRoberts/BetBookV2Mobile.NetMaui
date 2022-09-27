

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
[QueryProperty("ButtonColorStateModel", "ButtonColorStateModel")]
[QueryProperty("BetSlipState", "BetSlipState")]
public partial class GameDetailsViewModel : BaseViewModel
{
    private readonly BetSlipState _betSlipState;
    private readonly IUserService _userService;
    
    [ObservableProperty]
    GameDto gameDto;

    [ObservableProperty]
    ButtonColorStateModel buttonColorState;

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
        (BetSlip, ButtonColorState) = GetAllStates();
    }

    [RelayCommand]
    private void AddOrRemoveWagerForMoneyline(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.MONEYLINE);
        (BetSlip, ButtonColorState) = GetAllStates();
    }

    [RelayCommand]
    private void AddOrRemoveWagerForOverUnder(string overUnder)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(string.Concat(overUnder, GameDto.ScoreID.ToString()), GameDto, BetType.OVERUNDER);
        (BetSlip, ButtonColorState) = GetAllStates();
    }

    public (BetSlipStateModel, ButtonColorStateModel) GetAllStates()
    {
        BetSlip = _betSlipState.GetBetSlipState();
        ButtonColorState = _betSlipState.GetButtonColorState(GameDto);
        return (BetSlip, ButtonColorState);
    }

    [RelayCommand]
    private async Task GoToCurrentBetSlip()
    {
        await Shell.Current.GoToAsync(nameof(BetSlipPage), true);
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
