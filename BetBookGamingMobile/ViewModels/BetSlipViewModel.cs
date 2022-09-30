

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Handlers;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace BetBookGamingMobile.ViewModels;

public partial class BetSlipViewModel : BaseViewModel
{
    private readonly IMediator _mediator;

    public BetSlipViewModel(IMediator mediator)
	{
        _mediator = mediator;
    }

    [ObservableProperty]
    decimal parlyWagerAmount;

    [ObservableProperty]
    BetSlipState betSlipState;

    [RelayCommand]
    private async Task SubmitSinglesWager()
    {
        UserModel user = 
            await _mediator.Send(new GetUserByIdQuery("632395fdc17912bd030e4162"));

        BetSlipState =
            await _mediator.Send(new SubmitSinglesBetSlipCommand(user));
    }

    [RelayCommand]
    private async Task SubmitParleyWager()
    {
        UserModel user =
            await _mediator.Send(new GetUserByIdQuery("632395fdc17912bd030e4162"));

        BetSlipState = 
            await _mediator.Send(new SubmitParleyBetSlipCommand(user, ParlyWagerAmount));
    }

    [RelayCommand]
    async Task RemoveBetFromPreBets(CreateBetModel createBet)
    {
        BetSlipState = 
            await _mediator.Send(new DeleteBetCommand(createBet));
    }
}
