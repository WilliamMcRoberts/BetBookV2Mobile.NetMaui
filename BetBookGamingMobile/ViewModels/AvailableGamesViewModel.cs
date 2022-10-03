

using AndroidX.Lifecycle;
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using System.Collections.ObjectModel;

namespace BetBookGamingMobile.ViewModels;

public partial class AvailableGamesViewModel : BaseViewModel
{
    private readonly IConnectivity _connectivity;
    private readonly IMediator _mediator;

    public ObservableCollection<GameDto> Games { get; } = new();

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    SeasonType season;

    [ObservableProperty]
    int weekNumber;

    public AvailableGamesViewModel(IConnectivity connectivity,
                         IMediator mediator)
    {
        _connectivity = connectivity;
        _mediator = mediator;
    }

    [RelayCommand]
    public async Task GetGamesAsync(string text)
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

        Season = DateTime.Now.CalculateSeason();
        WeekNumber = Season.CalculateWeek(DateTime.Now);

        GameDto[] gamesArray =
            await _mediator.Send(new GetGamesByWeekAndSeasonQuery(WeekNumber, Season));

        if (gamesArray is null)
            return;

        if (Games.Count != 0)
            Games.Clear();

        foreach (var game in gamesArray.OrderBy(g => g.Date))
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
                    {"GameDto", gameDto }
                });
    }
}
