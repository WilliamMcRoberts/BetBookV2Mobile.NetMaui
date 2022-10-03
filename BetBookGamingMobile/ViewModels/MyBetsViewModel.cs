

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using System.Collections.ObjectModel;

namespace BetBookGamingMobile.ViewModels;

public partial class MyBetsViewModel : BaseViewModel
{
    List<SingleBetModel> bettorSingleBets = new();

    public ObservableCollection<GameDto> BettorSingleBetsInProgress { get; } = new();
    public ObservableCollection<GameDto> bettorSingleBetsWinners { get; } = new();
    public ObservableCollection<GameDto> bettorSingleBetsPush { get; } = new();
    public ObservableCollection<GameDto> bettorSingleBetsLosers { get; } = new();

    public MyBetsViewModel()
	{

	}

    // TODO -  Create Get Bettor Single Bets Method and Create Get Bettor Parley Bets Method
}
