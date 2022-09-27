﻿

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

    public MainViewModel(IConnectivity connectivity, 
                         IGameService gameService)
    {
        Title = "Bet Book Mobile";
        _connectivity = connectivity;
        _gameService = gameService;
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
            await _gameService.GetGamesByWeek(season, week);

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
        if (gameDto is null)
            return;

        await Shell.Current.GoToAsync(nameof(GameDetailsPage), true,
        new Dictionary<string, object>
                {
                    {"GameDto", gameDto },
                });
    }
}