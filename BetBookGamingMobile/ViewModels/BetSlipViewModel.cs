

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using static System.Net.Mime.MediaTypeNames;

namespace BetBookGamingMobile.ViewModels;

[QueryProperty("BetSlip", "BetSlip")]
public partial class BetSlipViewModel : BaseViewModel
{
	private readonly BetSlipState _betSlipState;
    private readonly IUserService _userService;
    private readonly IMediator _mediator;

    public BetSlipViewModel(BetSlipState betSlipState, IUserService userService, IMediator mediator)
	{
        _betSlipState = betSlipState;
        _userService = userService;
        _mediator = mediator;
    }

    [ObservableProperty]
    decimal parlyWagerAmount;

    [ObservableProperty]
    BetSlipStateModel betSlip;

    [RelayCommand]
    private async Task SubmitSinglesWager()
    {
        UserModel user = await _userService.GetUserByUserId("632395fdc17912bd030e4162");

        await _betSlipState.OnSubmitBetsFromSinglesBetSlip(user);
    }

    [RelayCommand]
    private async Task SubmitParleyWager()
    {
        UserModel user = await _userService.GetUserByUserId("632395fdc17912bd030e4162");

        bool parleyBetGood = await _betSlipState.OnSubmitBetsFromParleyBetSlip(user, parlyWagerAmount);
    }

    [RelayCommand]
    async Task RemoveBetFromPreBets(CreateBetModel createBet)
    {
        BetSlip = await _mediator.Send(new DeleteBetCommand(createBet));
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task GoBackToGames()
    {
        await Shell.Current.GoToAsync("../..");
    }
}
