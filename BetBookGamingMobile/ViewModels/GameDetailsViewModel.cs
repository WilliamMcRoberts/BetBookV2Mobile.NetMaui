

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BetBookGamingMobile.ViewModels;

[QueryProperty("GameDto", "GameDto")]
[QueryProperty("ButtonColorStateModel", "ButtonColorStateModel")]
public partial class GameDetailsViewModel : BaseViewModel
{
    private readonly BetSlipState _betSlipState;
    private readonly IUserService _userService;

    public ObservableCollection<CreateBetModel> Bets { get; } = new();

    [ObservableProperty]
    GameDto gameDto;

    [ObservableProperty]
    ButtonColorStateModel buttonColorStateModel;

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
        Bets.Clear();
        foreach (var createBet in _betSlipState.preBets)
            Bets.Add(createBet);
        ButtonColorStateModel = _betSlipState.GetButtonColorState(GameDto);
    }

    [RelayCommand]
    private void AddWagerForMoneyline(string winner)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(winner, GameDto, BetType.MONEYLINE);
        Bets.Clear();
        foreach (var createBet in _betSlipState.preBets)
            Bets.Add(createBet);
        ButtonColorStateModel = _betSlipState.GetButtonColorState(GameDto);
    }

    [RelayCommand]
    private void AddWagerForOverUnder(string overUnder)
    {
        _betSlipState.SelectOrRemoveWinnerAndGameForBet(string.Concat(overUnder, GameDto.ScoreID.ToString()), GameDto, BetType.OVERUNDER);
        Bets.Clear();
        foreach (var createBet in _betSlipState.preBets)
            Bets.Add(createBet);
        ButtonColorStateModel = _betSlipState.GetButtonColorState(GameDto);
    }

    [RelayCommand]
    private async Task SubmitWager()
    {
        UserModel user = await _userService.GetUserByUserId("632395fdc17912bd030e4162");

        await _betSlipState.OnSubmitBetsFromSinglesBetSlip(user);
    }
}
