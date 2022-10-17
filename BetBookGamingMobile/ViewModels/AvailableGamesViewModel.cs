
namespace BetBookGamingMobile.ViewModels;

public partial class AvailableGamesViewModel : AppBaseViewModel
{
    public ObservableCollection<GameDto> Games { get; } = new();

    [ObservableProperty]
    private bool isRefreshing;

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
        if (IsBusy) return;

        IsBusy = true;
        IsRefreshing = true;

        IEnumerable<GameDto> gameList = await _apiService.GetGames(Season, Week);

        Games.AddRange(
            gameList.Where(game => !game.HasStarted).OrderBy(game => game.Date), Games.Any());

        // Sample Data
        //Games.Add(new GameDto
        //{
        //    AwayTeam = "CIN",
        //    HomeTeam = "PIT",
        //    Date = new DateTime(2022, 10, 22, 11, 30, 00),
        //    PointSpread = (float)3.5,
        //    AwayTeamMoneyLine = -110,
        //    HomeTeamMoneyLine = 225,
        //    DateTime = new DateTime(2022, 10, 22, 1, 05, 00),
        //    OverUnder = (float)22.5,
        //    OverPayout = -110,
        //    UnderPayout = 325,
        //    ScoreID = 123456,
        //    PointSpreadAwayTeamMoneyLine = -110,
        //    PointSpreadHomeTeamMoneyLine = 225
        //});

        IsBusy = false;
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task GoToGameDetails(GameDto gameDto)
    {
        if (gameDto is null)
            return;

        await Shell.Current.GoToAsync(nameof(GameDetailsPage),
            true,
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
