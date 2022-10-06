

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
    int week;

    public AvailableGamesViewModel(IConnectivity connectivity,
                         IMediator mediator)
    {
        _connectivity = connectivity;
        _mediator = mediator;
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
            await _mediator.Send(new GetGamesByWeekAndSeasonQuery(Week, Season));

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

    [RelayCommand]
    private async Task SetStateAsync()
    {
        SetSeason();
        SetWeek();
        SetTitle();
        await GetGamesAsync();
    }

    private void SetSeason() =>
        Season = DateTime.Now.CalculateSeason();

    private void SetWeek() =>
        Week = Season.CalculateWeek(DateTime.Now);

    private void SetTitle() =>
        Title = Season == SeasonType.REG ? $"Regular Season Week {Week}"
             : Season == SeasonType.POST ? $"Post Season Week {Week}"
             : $"Pre Season Week {Week}";
}
