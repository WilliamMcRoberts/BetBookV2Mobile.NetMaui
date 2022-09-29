

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BetBookGamingMobile.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IConnectivity _connectivity;

    private readonly IGameService _gameService;

    //public bool gamesLoaded;

    public ObservableCollection<GameDto> Games { get; } = new();

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    SeasonType season;

    [ObservableProperty]
    int weekNumber;

    public MainViewModel(IConnectivity connectivity, 
                         IGameService gameService)
    {
        _connectivity = connectivity;
        _gameService = gameService;
    }

    [RelayCommand]
    public async Task GetGamesAsync()
    {
        if (IsBusy)
            return;

        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("No connectivity!",
                $"Please check internet and try again.", "OK");
            return;
        }

        IsBusy = true;

        GameDto[] gamesArray =
            await _gameService.GetGamesByWeek(Season, WeekNumber);

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
        //gamesLoaded = true;
    }

    [RelayCommand]
    private async Task GoToGameDetails(GameDto gameDto)
    {
        if (gameDto is null)
            return;

        await Shell.Current.GoToAsync(nameof(GameDetailsPage), true,
        new Dictionary<string, object>
                {
                    {"GameDto", gameDto },
                });
    }
}
