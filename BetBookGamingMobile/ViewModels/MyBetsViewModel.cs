

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

    [RelayCommand]
    public async Task GetAllBettorSingleBetsAsync()
    {
        string userId = _authenticationState.CurrentAuthenticationState.LoggedInUser.UserId;

        if (string.IsNullOrWhiteSpace(userId)) return;

        bettorSingleBets = await _mediator.Send(new GetBettorSingleBetsQuery(userId));

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.IN_PROGRESS))
            BettorSingleBetsInProgress.Add(singleBet);

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.WINNER))
            BettorSingleBetsWinners.Add(singleBet);

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.LOSER))
            BettorSingleBetsLosers.Add(singleBet);

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.PUSH))
            BettorSingleBetsPush.Add(singleBet);
    }

    [RelayCommand]
    public async Task GetAllBettorParleyBetsAsync()
    {
        string userId = _authenticationState.CurrentAuthenticationState.LoggedInUser.UserId;

        if (string.IsNullOrWhiteSpace(userId)) return;

        bettorParleyBets = await _mediator.Send(new GetBettorParleyBetsQuery(userId));

        foreach (var parleyBet in bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.IN_PROGRESS))
            BettorParleyBetsInProgress.Add(parleyBet);

        foreach (var parleyBet in bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.WINNER))
            BettorParleyBetsWinners.Add(parleyBet);

        foreach (var parleyBet in bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.LOSER))
            BettorParleyBetsLosers.Add(parleyBet);

        foreach (var parleyBet in bettorParleyBets.Where(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.PUSH))
            BettorParleyBetsPush.Add(parleyBet);
    }
}
