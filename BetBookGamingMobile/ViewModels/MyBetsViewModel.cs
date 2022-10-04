

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using System.Collections.ObjectModel;

namespace BetBookGamingMobile.ViewModels;

public partial class MyBetsViewModel : BaseViewModel
{
    private readonly AuthenticationState _authenticationState;
    private readonly IMediator _mediator;

    public List<SingleBetModel> bettorSingleBets = new();
    public ObservableCollection<SingleBetModel> BettorSingleBetsInProgress { get; } = new();
    public ObservableCollection<SingleBetModel> BettorSingleBetsWinners { get; } = new();
    public ObservableCollection<SingleBetModel> BettorSingleBetsPush { get; } = new();
    public ObservableCollection<SingleBetModel> BettorSingleBetsLosers { get; } = new();

    public List<ParleyBetSlipModel> bettorParleyBets = new();
    public ObservableCollection<ParleyBetSlipModel> BettorParleyBetsInProgress { get; } = new();
    public ObservableCollection<ParleyBetSlipModel> BettorParleyBetsWinners { get; } = new();
    public ObservableCollection<ParleyBetSlipModel> BettorParleyBetsPush { get; } = new();
    public ObservableCollection<ParleyBetSlipModel> BettorParleyBetsLosers { get; } = new();

    public MyBetsViewModel(AuthenticationState authenticationState, IMediator mediator)
	{
        _authenticationState = authenticationState;
        _mediator = mediator;
    }

    public async Task GetAllBettorSingleBetsAsync()
    {
        AuthenticationStateModel authState = _authenticationState.GetCurrentAuthenticationState();

        if (authState.LoggedInUser is null) return;

        bettorSingleBets = await _mediator.Send(new GetBettorSingleBetsQuery(authState.LoggedInUser.UserId));

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.IN_PROGRESS))
            BettorSingleBetsInProgress.Add(singleBet);

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.WINNER))
            BettorSingleBetsWinners.Add(singleBet);

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.LOSER))
            BettorSingleBetsLosers.Add(singleBet);

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.PUSH))
            BettorSingleBetsPush.Add(singleBet);
    }

    public async Task GetAllBettorParleyBetsAsync()
    {
        AuthenticationStateModel authState = _authenticationState.GetCurrentAuthenticationState();

        if (authState.LoggedInUser is null) return;

        bettorParleyBets = await _mediator.Send(new GetBettorParleyBetsQuery(authState.LoggedInUser.UserId));

        foreach (var parleyBet in bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.IN_PROGRESS))
            BettorParleyBetsInProgress.Add(parleyBet);

        foreach (var parleyBet in bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.WINNER))
            BettorParleyBetsWinners.Add(parleyBet);

        foreach (var parleyBet in bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.LOSER))
            BettorParleyBetsLosers.Add(parleyBet);

        foreach (var parleyBet in bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.PUSH))
            BettorParleyBetsPush.Add(parleyBet);
    }

    [RelayCommand]
    private async Task SetStateAsync()
    {
        await GetAllBettorSingleBetsAsync();
        await GetAllBettorParleyBetsAsync();
    }
}
