

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace BetBookGamingMobile.ViewModels;

[QueryProperty("BetSlip", "BetSlip")]
public partial class BetSlipViewModel : BaseViewModel
{
	private readonly BetSlip _betSlip;
    private readonly IUserService _userService;
    private readonly IMediator _mediator;

    public BetSlipViewModel(BetSlip betSlip, IUserService userService, IMediator mediator)
	{
        _betSlip = betSlip;
        _userService = userService;
        _mediator = mediator;
    }

    [ObservableProperty]
    decimal parlyWagerAmount;

    [ObservableProperty]
    BetSlipState betSlipState;

    [RelayCommand]
    private async Task SubmitSinglesWager()
    {
        UserModel user = await _userService.GetUserByUserId("632395fdc17912bd030e4162");

        await _betSlip.OnSubmitBetsFromSinglesBetSlip(user);
    }

    [RelayCommand]
    private async Task SubmitParleyWager()
    {
        UserModel user = await _userService.GetUserByUserId("632395fdc17912bd030e4162");

        bool parleyBetGood = await _betSlip.OnSubmitBetsFromParleyBetSlip(user, parlyWagerAmount);
    }

    [RelayCommand]
    async Task RemoveBetFromPreBets(CreateBetModel createBet)
    {
        BetSlipState = await _mediator.Send(new DeleteBetCommand(createBet));
    }
}
