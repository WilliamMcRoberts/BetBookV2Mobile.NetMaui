

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BetBookGamingMobile.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IConnectivity _connectivity;
    private readonly IGameService _gameService;
    private readonly BetSlipState _betSlipState;

    public MainViewModel(IConnectivity connectivity, IGameService gameService, BetSlipState betSlipState)
    {
        Title = "Bet Book Mobile";
        _connectivity = connectivity;
        _gameService = gameService;
        _betSlipState = betSlipState;
    }

    public ObservableCollection<GameDto> Games { get; } = new();

    [ObservableProperty]
    private bool isRefreshing;

    [RelayCommand]
    private async Task GetGamesAsync()
    {
        if (IsBusy)
            return;

        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("No connectivity!",
                $"Please check internet and try again.", "OK");
            return;
        }

        SeasonType season = DateTime.Now.CalculateSeason();
        int week = season.CalculateWeek(DateTime.Now);

        IsBusy = true;

        GameDto[] gamesArray =
            await _gameService.GetGamesByWeekAndSeason(season, week);

        if (gamesArray is null)
            return;

        gamesArray = gamesArray.OrderBy(g => g.Date).ToArray();

        if (Games.Count != 0)
            Games.Clear();

        foreach (var game in gamesArray)
            if (!game.HasStarted)
                Games.Add(game);

        IsBusy = false;
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task GoToGameDetails(GameDto gameDto)
    {
        if (gameDto == null)
            return;

        ButtonColorStateModel buttonColorStateModel = 
            _betSlipState.GetButtonColorState(gameDto);

        await Shell.Current.GoToAsync(nameof(GameDetailsPage), true,
        new Dictionary<string, object>
                {
                    {"GameDto", gameDto },
                    {"ButtonColorStateModel", buttonColorStateModel }
                });
    }
}
