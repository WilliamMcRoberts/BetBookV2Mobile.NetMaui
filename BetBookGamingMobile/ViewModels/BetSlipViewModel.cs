

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

    public BetSlipViewModel(IMediator mediator)
	{
        _mediator = mediator;
    }

    //[ObservableProperty]
    //decimal parlyWagerAmount;

    [ObservableProperty]
    BetSlipState betSlipState;

    [RelayCommand]
    private async Task SubmitSinglesWager()
    {
        UserModel user = 
            await _mediator.Send(new GetUserByIdQuery("632395fdc17912bd030e4162"));

        if (user is null || user.AccountBalance == 0)
            return;

        bool singlesBetSlipGood = await _mediator.Send(new SubmitBetsFromSinglesBetSlipCommand(user));

        if (singlesBetSlipGood) BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());
    }

    [RelayCommand]
    private async Task SubmitParleyWager(decimal parleyWagerAmount)
    {
        UserModel user =
            await _mediator.Send(new GetUserByIdQuery("632395fdc17912bd030e4162"));

        if (user is null || user.AccountBalance == 0)
            return;

        bool parleyBetSlipGood = await _mediator.Send(new SubmitBetsFromParleyBetSlipCommand(user, parleyWagerAmount));

        if (parleyBetSlipGood) BetSlipState = await _mediator.Send(new GetBetSlipStateQuery());

        parleyWagerAmount = 0;
    }

    [RelayCommand]
    async Task RemoveBetFromPreBets(CreateBetModel createBet) => 
        BetSlipState = await _mediator.Send(new DeleteBetCommand(createBet));
}
