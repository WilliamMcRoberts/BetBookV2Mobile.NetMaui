
using GoogleGson;
using MonkeyCache.FileStore;

namespace BetBookGamingMobile.ViewModels;

public partial class AvailableGamesViewModel : AppBaseViewModel
{
    public ObservableCollection<GameDto> Games { get; } = new();

    [ObservableProperty]
    private SeasonType _season;

    [ObservableProperty]
    private int _week;

    public AvailableGamesViewModel(IApiService apiService) :base(apiService)
    {
    }

    [RelayCommand]
    public async Task GetGamesAsync()
    {
        var json = String.Empty;

        json = Barrel.Current.Get<string>("games");

        if (string.IsNullOrWhiteSpace(json) || Barrel.Current.IsExpired("games"))
        {
            Barrel.Current.EmptyExpired();

            if(isNotBusy)
                await RefreshGames();

            return;
        }

        IEnumerable<GameDto> gameListFromCache = JsonSerializer.Deserialize<IEnumerable<GameDto>>(json);
        Games.AddRange(gameListFromCache.Where(game => !game.HasStarted).OrderBy(game => game.Date), Games.Any());
    }

    [RelayCommand]
    private async Task RefreshGames()
    {
        if (!IsRefreshing) IsBusy = true;

        var json = String.Empty;

        try
        {
            IEnumerable<GameDto> gameList = await _apiService.GetGames(Season, Week);

            if (gameList is null) return;

            json = JsonSerializer.Serialize(gameList);

            Games.AddRange(gameList.Where(game => !game.HasStarted).OrderBy(game => game.Date), Games.Any());

            Barrel.Current.Add(key: "games", json, TimeSpan.FromMinutes(10));
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert(
                "Something went wrong", "Please try again in a moment", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
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
    protected async Task SetStateAsync()
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
