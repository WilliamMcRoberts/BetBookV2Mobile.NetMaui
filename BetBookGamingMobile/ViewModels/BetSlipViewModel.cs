

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace BetBookGamingMobile.ViewModels;

public partial class BetSlipViewModel : BaseViewModel
{
    private readonly IMediator _mediator;
    private readonly AuthState _authState;

    public BetSlipViewModel(IMediator mediator, AuthState authState)
	{
        _mediator = mediator;
        _authState = authState;
    }

    [ObservableProperty]
    decimal parlyWagerAmount;

    [ObservableProperty]
    BetSlipState betSlipState;

    [RelayCommand]
    private async Task SubmitSinglesWager()
    {
        if (_authState.LoggedInUser is null)
            return;

        bool singlesBetSlipGood = 
            await _mediator.Send(new SubmitBetsFromSinglesBetSlipCommand(_authState.LoggedInUser));

        if (singlesBetSlipGood) 
            BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());
    }

    [RelayCommand]
    private async Task SubmitParleyWager(decimal parleyWagerAmount)
    {
        if (_authState.LoggedInUser is null)
            return;

        bool parleyBetSlipGood = await _mediator.Send(new SubmitBetsFromParleyBetSlipCommand(
                _authState.LoggedInUser, parleyWagerAmount));

        if (parleyBetSlipGood) 
            BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());

        parleyWagerAmount = 0;
    }

    [RelayCommand]
    async Task RemoveBetFromPreBets(CreateBetModel createBet) => 
        BetSlipState = await _mediator.Send(new DeleteBetCommand(createBet));

    [RelayCommand]
    async Task GetBetSlipStateAsync() =>
        BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());
}
