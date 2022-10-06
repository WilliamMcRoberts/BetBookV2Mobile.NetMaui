

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.GlobalStateManagement;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

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
    decimal parleyWagerAmount;

    [ObservableProperty]
    BetSlipStateModel betSlipState;

    [RelayCommand]
    private async Task SubmitSinglesWagerAsync()
    {
        var authState = await _mediator.Send(new GetCurrentAuthenticationStateQuery());

        LoggedInUser = authState.LoggedInUser;        
            
        if (string.IsNullOrEmpty(LoggedInUser.UserId)) return;

        bool singlesBetSlipGood = 
            await _mediator.Send(new SubmitBetsFromSinglesBetSlipCommand(LoggedInUser));

        if (singlesBetSlipGood) 
            BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());
    }

    [RelayCommand]
    private async Task SubmitParleyWagerAsync()
    {
        var authState = await _mediator.Send(new GetCurrentAuthenticationStateQuery());

        LoggedInUser = authState.LoggedInUser;

        if (string.IsNullOrEmpty(LoggedInUser.UserId)) return;

        bool parleyBetSlipGood = await _mediator.Send(new SubmitBetsFromParleyBetSlipCommand(
                LoggedInUser, ParleyWagerAmount));

        if (parleyBetSlipGood) 
            BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());

        ParleyWagerAmount = 0;
    }

    [RelayCommand]
    private async Task RemoveBetFromPreBets(CreateBetModel createBet) =>
        BetSlipState = await _mediator.Send(new DeleteBetCommand(createBet));

    [RelayCommand]
    private async Task SetStateAsync() =>
        BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());
}
