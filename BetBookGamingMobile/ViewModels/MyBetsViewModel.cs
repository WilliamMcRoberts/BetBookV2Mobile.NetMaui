

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.GlobalStateManagement;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using System.Collections.ObjectModel;

namespace BetBookGamingMobile.ViewModels;

public partial class MyBetsViewModel : BaseViewModel
{
    private readonly IMediator _mediator;
    private AuthenticationStateModel _currentAuthenticationState;

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

    public MyBetsViewModel(IMediator mediator)
	{
        _mediator = mediator;
    }

    [RelayCommand]
    private async Task SetStateAsync()
    {
        _currentAuthenticationState =
             await _mediator.Send(new GetCurrentAuthenticationStateQuery());

        if (string.IsNullOrEmpty(_currentAuthenticationState.LoggedInUser.UserId))
            return;

        await GetAllBettorSingleBetsAsync();
        await GetAllBettorParleyBetsAsync();
    }

    private async Task GetAllBettorSingleBetsAsync()
    {
        bettorSingleBets = await _mediator.Send(
            new GetBettorSingleBetsQuery(_currentAuthenticationState.LoggedInUser.UserId));

        PopulateSingleBetObservableCollectionsFromBettorSingleBetsList(bettorSingleBets);
    }

    private async Task GetAllBettorParleyBetsAsync()
    {
        bettorParleyBets = await _mediator.Send(
            new GetBettorParleyBetsQuery(_currentAuthenticationState.LoggedInUser.UserId));

        PopulateParleyBetObservableCollectionsFromBettorParleyBetsList(bettorParleyBets);
    }

    private void PopulateSingleBetObservableCollectionsFromBettorSingleBetsList(List<SingleBetModel> bettorSingleBets)
    {
        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.IN_PROGRESS))
            BettorSingleBetsInProgress.Add(singleBet);

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.WINNER))
            BettorSingleBetsWinners.Add(singleBet);

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.LOSER))
            BettorSingleBetsLosers.Add(singleBet);

        foreach (var singleBet in bettorSingleBets.Where(b => b.SingleBetStatus == SingleBetStatus.PUSH))
            BettorSingleBetsPush.Add(singleBet);
    }

    private void PopulateParleyBetObservableCollectionsFromBettorParleyBetsList(List<ParleyBetSlipModel> bettorParleyBets)
    {
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
