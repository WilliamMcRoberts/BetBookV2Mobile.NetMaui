
using MonkeyCache.FileStore;

namespace BetBookGamingMobile.ViewModels;

public partial class AvailableGamesViewModel : AppBaseViewModel
{
    public ObservableCollection<GameDto> Games { get; } = new();

    [ObservableProperty]
    SeasonType season;

    [ObservableProperty]
    int week;

    public AvailableGamesViewModel(IApiService apiService) :base(apiService)
    {
    }

    [RelayCommand]
    public async Task GetGamesAsync()
    {
        if (IsBusy)
        {
            await Shell.Current.DisplayAlert(
                "App is busy", "Application is busy doing something else...please try again in a moment", "OK");
            return;
        }

        IsBusy = true;

        try
        {
            var json = String.Empty;

            json = Barrel.Current.Get<string>("games");

            if (!string.IsNullOrWhiteSpace(json))
            {
                IEnumerable<GameDto> gameListFromCache = JsonSerializer.Deserialize<IEnumerable<GameDto>>(json);
                Games.AddRange(
                    gameListFromCache.Where(game => !game.HasStarted).OrderBy(game => game.Date), Games.Any());
                return;
            }

            IEnumerable<GameDto> gameList = await _apiService.GetGames(Season, Week);

            json = JsonSerializer.Serialize(gameList);

            Barrel.Current.Add("games", json, TimeSpan.FromMinutes(15));

            Games.AddRange(gameList.Where(game => !game.HasStarted).OrderBy(game => game.Date), Games.Any());
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Something went wrong", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
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
