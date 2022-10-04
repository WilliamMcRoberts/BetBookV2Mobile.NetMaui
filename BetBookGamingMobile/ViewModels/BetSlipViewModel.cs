

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using System.Collections.ObjectModel;

namespace BetBookGamingMobile.ViewModels;

public partial class BetSlipViewModel : BaseViewModel
{
    private readonly IMediator _mediator;
    private readonly AuthenticationState _authenticationState;

    public BetSlipViewModel(IMediator mediator, AuthenticationState authenticationState)
	{
        _mediator = mediator;
        _authenticationState = authenticationState;
    }

    [ObservableProperty]
    decimal parlyWagerAmount;

    [ObservableProperty]
    BetSlipStateModel betSlipState;

    [RelayCommand]
    private async Task SubmitSinglesWager()
    {
        if (_authenticationState.CurrentAuthenticationState.LoggedInUser is null)
            return;

        bool singlesBetSlipGood = 
            await _mediator.Send(new SubmitBetsFromSinglesBetSlipCommand(_authenticationState.CurrentAuthenticationState.LoggedInUser));

        if (singlesBetSlipGood) 
            BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());
    }

    [RelayCommand]
    private async Task SubmitParleyWager(decimal parleyWagerAmount)
    {
        if (_authenticationState.CurrentAuthenticationState.LoggedInUser is null)
            return;

        bool parleyBetSlipGood = await _mediator.Send(new SubmitBetsFromParleyBetSlipCommand(
                _authenticationState.CurrentAuthenticationState.LoggedInUser, parleyWagerAmount));

        if (parleyBetSlipGood) 
            BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());

        parleyWagerAmount = 0;
    }

    [RelayCommand]
    private async Task RemoveBetFromPreBets(CreateBetModel createBet) =>
        await _mediator.Send(new DeleteBetCommand(createBet));

    [RelayCommand]
    private async Task SetStateAsync() =>
        BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());
}
