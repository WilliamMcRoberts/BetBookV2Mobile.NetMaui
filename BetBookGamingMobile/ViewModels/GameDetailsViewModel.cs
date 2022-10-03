

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Handlers;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace BetBookGamingMobile.ViewModels;

[QueryProperty("GameDto", "GameDto")]
public partial class GameDetailsViewModel : BaseViewModel
{
    private readonly IMediator _mediator;

    [ObservableProperty]
    GameDto gameDto;

    [ObservableProperty]
    ButtonColorState buttonColorState;

    [ObservableProperty]
    ButtonTextState buttonTextState;

    [ObservableProperty]
    BetSlipState betSlipState;

    public GameDetailsViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }    

    [RelayCommand]
    private async Task AddOrRemoveWagerForPointSpreadAsync(string winner)
    {
        BetSlipState = await _mediator.Send(
            new SelectOrRemoveWinnerAndGameForBetCommand(winner, GameDto, BetType.POINTSPREAD));
        ButtonColorState = await _mediator.Send(new GetButtonColorStateQuery(GameDto));
    }

    [RelayCommand]
    private async Task AddOrRemoveWagerForMoneylineAsync(string winner)
    {
        BetSlipState = await _mediator.Send(
                new SelectOrRemoveWinnerAndGameForBetCommand(winner, GameDto, BetType.MONEYLINE));
        ButtonColorState = await _mediator.Send(new GetButtonColorStateQuery(GameDto));
    }

    [RelayCommand]
    private async Task AddOrRemoveWagerForOverUnderAsync(string winner)
    {
        BetSlipState = await _mediator.Send(new SelectOrRemoveWinnerAndGameForBetCommand(
            string.Concat(winner, GameDto.ScoreID.ToString()), GameDto, BetType.OVERUNDER));
        ButtonColorState = await _mediator.Send(new GetButtonColorStateQuery(GameDto));
    }
}
