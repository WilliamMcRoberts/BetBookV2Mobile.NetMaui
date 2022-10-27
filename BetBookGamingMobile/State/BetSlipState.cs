
namespace BetBookGamingMobile.State;

public class BetSlipState
{
    public List<CreateBetModel> preBets = new();
    public UserModel loggedInUser;
    public bool conflictingBetsForParley;
    public decimal totalWagerForParley;
    public bool parleyBetAmountBad;
    public bool gameHasStarted;
    public bool betAmountForSinglesBad;
    public bool betAmountForParleyBad;
    public SeasonType season;
    public int week;
    public string startedGameDescription;

    private readonly IApiService _apiService;

    public BetSlipState(IApiService apiService)
    {
        _apiService = apiService;
    }

    public void SelectOrRemoveWinnerAndGameForBet(
        string winner, GameDto game, BetType betType)
    {
        var bet = preBets.Where(b => b.Winner == winner && b.Game.ScoreID == game.ScoreID && b.BetType == betType)
            .FirstOrDefault()!;

        if (preBets.Contains(bet))
        {
            preBets.Remove(bet);
            conflictingBetsForParley = CheckForConflictingBets();
            return;
        }

        preBets.Add(new CreateBetModel
        {
            BetType = betType,
            MoneylinePayout = winner.GetMoneylinePayoutForBet(game, betType),
            Game = game,
            Winner = winner,
            PointSpread = Math.Round((decimal)game.PointSpread, 1),
            OverUnder = Math.Round((decimal)game.OverUnder, 1),
        });

        conflictingBetsForParley = CheckForConflictingBets();
    }

    public bool CheckForConflictingBets()
    {
        foreach (var bet in preBets)
            if (preBets.Where(b => b.Game.ScoreID == bet.Game.ScoreID && b.BetType == bet.BetType).Count() > 1)
                return conflictingBetsForParley = true;

        return conflictingBetsForParley = false;
    }

    public async Task<bool> OnSubmitBetsFromSinglesBetSlip(UserModel loggedInUser)
    {
        if (preBets.Count < 1) return false;

        season = DateTime.Now.CalculateSeason();
        week = season.CalculateWeek(DateTime.Now);

        IEnumerable<GameDto> gameCheckList =
            await _apiService.GetGames(season, week);

        foreach (var bet in preBets)
        {
            if (string.IsNullOrEmpty(bet.BetAmount) || !decimal.TryParse(bet.BetAmount, out var betAmount))
            {
                betAmountForSinglesBad = true;
                return false;
            }

            GameDto game = gameCheckList.Where(
                g => g.ScoreID == bet.Game.ScoreID).FirstOrDefault()!;

            if (game.HasStarted)
            {
                gameHasStarted = true;
                startedGameDescription = $"{game.AwayTeam} at {game.HomeTeam}";
                return false;
            }

            var singleBet = new SingleBetModel
            {
                WinnerChosen = bet.BetType == BetType.OVERUNDER ?
                               (bet.Winner[0] == 'O' ? "Over" : "Under")
                                : bet.Winner,

                BetPayout =
                    betAmount.CalculateSingleBetPayout(bet.MoneylinePayout),

                BettorId = loggedInUser.UserId,
                BetType = bet.BetType,
                BetAmount = betAmount,
                SingleBetStatus = SingleBetStatus.IN_PROGRESS,
                SingleBetPayoutStatus = SingleBetPayoutStatus.UNPAID,
                GameSnapshot = bet.Game.GetGameSnapshot(),
                PointsAfterSpread = bet.Game.CalculatePointsAfterSpread(bet.Winner)
            };

            bool singleBetGood = 
                await _apiService.CreateSingleBet(singleBet);

            if(!singleBetGood)
                return false;
        }

        preBets.Clear();
        return true;
    }

    public async Task<bool> OnSubmitBetsFromParleyBetSlip(
            UserModel loggedInUser, decimal parleyWagerAmount)
    {
        if (preBets.Count < 1 || conflictingBetsForParley)
            return false;

        if (parleyWagerAmount <= 0)
        {
            betAmountForParleyBad = true;
            return false;
        }

        season = DateTime.Now.CalculateSeason();
        week = season.CalculateWeek(DateTime.Now);

        IEnumerable<GameDto> gameCheckList =
            await _apiService.GetGames(season, week);

        var parleyBetSlip = new ParleyBetSlipModel();

        foreach (var bet in preBets)
        {
            GameDto game = gameCheckList.Where(
                g => g.ScoreID == bet.Game.ScoreID).FirstOrDefault()!;

            if (game.HasStarted)
            {
                gameHasStarted = true;
                startedGameDescription = $"{game.AwayTeam} at {game.HomeTeam}";
                return false;
            }

            parleyBetSlip.SingleBetsForParleyList.Add(bet.GetSingleBetForParley(loggedInUser));
        }

        parleyBetSlip.BettorId = loggedInUser.UserId;
        parleyBetSlip.ParleyBetAmount = parleyWagerAmount;
        parleyBetSlip.ParleyBetPayout = GetPayoutForTotalBetsParley(parleyWagerAmount);
        parleyBetSlip.ParleyBetSlipStatus = ParleyBetSlipStatus.IN_PROGRESS;
        parleyBetSlip.ParleyBetSlipPayoutStatus = ParleyBetSlipPayoutStatus.UNPAID;

        bool parleyBetGood =
            await _apiService.CreateParleyBet(parleyBetSlip);

        preBets.Clear();

        return parleyBetGood;
    }

    public decimal GetPayoutForTotalBetsParley(decimal totalParleyWager)
    {
        if (preBets.Count < 2) return 0;

        decimal totalDecimalOdds = 1;

        preBets.ForEach(
            bet => totalDecimalOdds *= bet.MoneylinePayout.ConvertMoneylinePayoutToDecimalFormat());

        return Math.Round(totalParleyWager * totalDecimalOdds, 2);
    }

    public decimal GetPayoutForTotalBetsSingles()
    {
        decimal total = 0;

        preBets.ForEach(bet => total += decimal.TryParse(bet.BetAmount, out var betAmount) ?
                betAmount.CalculateSingleBetPayout(bet.MoneylinePayout) : 0);

        return total;
    }
}


