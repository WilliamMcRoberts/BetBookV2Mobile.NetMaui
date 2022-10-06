

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
        var authState = await _mediator.Send(new GetCurrentAuthenticationStateQuery());

        LoggedInUser = authState.LoggedInUser;

        if (string.IsNullOrEmpty(LoggedInUser.UserId)) return;

        AddSinglesDelegate addSingles = AddSingleBetsToObservableCollection;
        AddParleysDelegate addParleys = AddParleyBetsToObservableCollection;

        await GetAllBettorSingleBetsAsync(addSingles);
        await GetAllBettorParleyBetsAsync(addParleys);
    }

    private async Task GetAllBettorSingleBetsAsync(AddSinglesDelegate add)
    {
        bettorSingleBets = await _mediator.Send(
            new GetBettorSingleBetsQuery(LoggedInUser.UserId));

        add(BettorSingleBetsInProgress, SingleBetStatus.IN_PROGRESS);
        add(BettorSingleBetsWinners, SingleBetStatus.WINNER);
        add(BettorSingleBetsLosers, SingleBetStatus.LOSER);
        add(BettorSingleBetsPush, SingleBetStatus.PUSH);
    }

    private async Task GetAllBettorParleyBetsAsync(AddParleysDelegate add)
    {
        bettorParleyBets = await _mediator.Send(
            new GetBettorParleyBetsQuery(LoggedInUser.UserId));

        add(BettorParleyBetsInProgress, ParleyBetSlipStatus.IN_PROGRESS);
        add(BettorParleyBetsWinners, ParleyBetSlipStatus.WINNER);
        add(BettorParleyBetsLosers, ParleyBetSlipStatus.LOSER);
        add(BettorParleyBetsPush, ParleyBetSlipStatus.PUSH);
    }

    private delegate void AddSinglesDelegate(
        ObservableCollection<SingleBetModel> collection, SingleBetStatus status);

    private delegate void AddParleysDelegate(
        ObservableCollection<ParleyBetSlipModel> collection, ParleyBetSlipStatus status);

    private void AddSingleBetsToObservableCollection(
        ObservableCollection<SingleBetModel> collection, SingleBetStatus status)
    {
        foreach (var bet in bettorSingleBets.Where(b => b.SingleBetStatus == status))
            collection.Add(bet);
    }

    private void AddParleyBetsToObservableCollection(
        ObservableCollection<ParleyBetSlipModel> collection, ParleyBetSlipStatus status)
    {
        foreach (var bet in bettorParleyBets.Where(b => b.ParleyBetSlipStatus == status))
            collection.Add(bet);
    }
}
