using BetBookGamingMobile.Helpers;
namespace BetBookGamingMobile.GlobalStateManagement;

public class BetSlipState
{
    public List<CreateBetModel> preBets = new();
    public UserModel loggedInUser;
    public bool conflictingBetsForParley;
    public decimal totalWagerForParley;
    public decimal totalPayoutForParley;
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

    public (ButtonColorStateModel, BetSlipStateModel) SelectOrRemoveWinnerAndGameForBet(
        string winner, GameDto game, BetType betType)
    {
        if (preBets.Contains(preBets.Where(b => b.Winner == winner && b.Game.ScoreID == game.ScoreID && b.BetType == betType)
                   .FirstOrDefault()!))
        {
            preBets.Remove(
            preBets.Where(b => b.Winner == winner && b.Game.ScoreID == game.ScoreID && b.BetType == betType)
                   .FirstOrDefault()!);

            conflictingBetsForParley = CheckForConflictingBets();
            return (GetButtonColorState(game), GetBetSlipState());
        }

        preBets.Add(new CreateBetModel
        {
            BetType = betType,
            BetAmount = 0,
            MoneylinePayout = winner.GetMoneylinePayoutForBet(game, betType),
            Game = game,
            Winner = winner,
            PointSpread = Math.Round(Convert.ToDecimal(game.PointSpread), 1),
            OverUnder = Math.Round(Convert.ToDecimal(game.OverUnder), 1),
        });

        conflictingBetsForParley = CheckForConflictingBets();
        return (GetButtonColorState(game), GetBetSlipState());
    }

    public (BetSlipStateModel, ButtonColorStateModel, ButtonTextStateModel) GetAllStates(GameDto gameDto) =>
        (GetBetSlipState(), GetButtonColorState(gameDto), GetButtonTextState(gameDto));

    public ButtonColorStateModel GetButtonColorState(GameDto gameDto) =>
        new()
        {
            ApColor = preBets.Contains(preBets.Where(b => 
                b.Winner == gameDto.AwayTeam && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.POINTSPREAD)
                .FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue,
            HpColor = preBets.Contains(preBets.Where(b => 
                b.Winner == gameDto.HomeTeam && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.POINTSPREAD)
                .FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue,
            AmColor = preBets.Contains(preBets.Where(b => 
                b.Winner == gameDto.AwayTeam && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.MONEYLINE)
                .FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue,
            HmColor = preBets.Contains(preBets.Where(b => 
                b.Winner == gameDto.HomeTeam && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.MONEYLINE)
                .FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue,
            OColor = preBets.Contains(preBets.Where(b => 
                b.Winner == string.Concat("Over", gameDto.ScoreID.ToString()) && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.OVERUNDER)
                .FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue,
            UColor = preBets.Contains(preBets.Where(b => 
                b.Winner == string.Concat("Under", gameDto.ScoreID.ToString()) && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.OVERUNDER)
                .FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue
        };

    public ButtonTextStateModel GetButtonTextState(GameDto gameDto) =>
        new()
        {
            ApText = $"{gameDto.AwayTeam} {gameDto.AwayTeamPointSpreadForDisplay}    {gameDto.PointSpreadAwayTeamMoneyLine}",
            HpText = $"{gameDto.HomeTeam} {gameDto.HomeTeamPointSpreadForDisplay}    {gameDto.PointSpreadHomeTeamMoneyLine}",
            AmText = $"{gameDto.AwayTeamMoneyLine}",
            HmText = $"{gameDto.HomeTeamMoneyLine}",
            OText = $"Over {gameDto.OverUnder}    {gameDto.OverPayout}",
            UText = $"Under {gameDto.OverUnder}    {gameDto.UnderPayout}"
        };

    public BetSlipStateModel GetBetSlipState()
    {
        var betSlipState = new BetSlipStateModel();

        betSlipState.BetsInBetSlip.AddRange(preBets);

        return betSlipState;
    }

    public bool CheckForConflictingBets()
    {
        foreach (CreateBetModel cb in preBets)
        {
            if (preBets.Where(b => b.Game.ScoreID == cb.Game.ScoreID && b.BetType == cb.BetType).Count() > 1)
                return true;
        }

        return false;
    }

    public async Task<bool> OnSubmitBetsFromSinglesBetSlip(UserModel loggedInUser)
    {
        if (preBets.Count < 1) return false;

        season = DateTime.Now.CalculateSeason();
        week = season.CalculateWeek(DateTime.Now);

        IEnumerable<GameDto> gameCheckList =
            await _apiService.GetGames(season, week);

        foreach (CreateBetModel createBetModel in preBets)
        {
            if (createBetModel.BetAmount <= 0)
            {
                betAmountForSinglesBad = true;
                return false;
            }

            GameDto game = gameCheckList.Where(
                g => g.ScoreID == createBetModel.Game.ScoreID).FirstOrDefault()!;

            if (game.HasStarted)
            {
                gameHasStarted = true;
                startedGameDescription = $"{game.AwayTeam} at {game.HomeTeam}";
                return false;
            }

            var singleBet = new SingleBetModel
            {
                WinnerChosen = createBetModel.BetType == BetType.OVERUNDER ?
                               (createBetModel.Winner[0] == 'O' ? "Over" : "Under")
                                : createBetModel.Winner,

                BetPayout =
                    Math.Round(createBetModel.BetAmount.CalculateSingleBetPayout(createBetModel.MoneylinePayout), 2),

                BettorId = loggedInUser.UserId,
                BetType = createBetModel.BetType,
                BetAmount = createBetModel.BetAmount,
                SingleBetStatus = SingleBetStatus.IN_PROGRESS,
                SingleBetPayoutStatus = SingleBetPayoutStatus.UNPAID,
                GameSnapshot = CreateGameSnapshot(createBetModel.Game),
                PointsAfterSpread = createBetModel.Game.CalculatePointsAfterSpread(createBetModel.Winner)
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

        foreach (CreateBetModel createBetModel in preBets)
        {
            GameDto game = gameCheckList.Where(
                g => g.ScoreID == createBetModel.Game.ScoreID).FirstOrDefault()!;

            if (game.HasStarted)
            {
                gameHasStarted = true;
                startedGameDescription = $"{game.AwayTeam} at {game.HomeTeam}";
                return false;
            }

            parleyBetSlip.SingleBetsForParleyList.Add(new SingleBetForParleyModel
            {
                WinnerChosen = createBetModel.BetType == BetType.OVERUNDER ?
                               (createBetModel.Winner[0] == 'O' ? "Over" : "Under")
                                : createBetModel.Winner,

                PointsAfterSpread =
                    createBetModel.Game.CalculatePointsAfterSpread(createBetModel.Winner),

                BettorId = loggedInUser.UserId,
                BetType = createBetModel.BetType,
                SingleBetForParleyStatus = SingleBetForParleyStatus.IN_PROGRESS,
                GameSnapshot = CreateGameSnapshot(createBetModel.Game)
            });
        }

        parleyBetSlip.BettorId = loggedInUser.UserId;
        parleyBetSlip.ParleyBetAmount = parleyWagerAmount;
        parleyBetSlip.ParleyBetPayout = Math.Round(GetPayoutForTotalBetsParley(parleyWagerAmount), 2);
        parleyBetSlip.ParleyBetSlipStatus = ParleyBetSlipStatus.IN_PROGRESS;
        parleyBetSlip.ParleyBetSlipPayoutStatus = ParleyBetSlipPayoutStatus.UNPAID;

        bool parleyBetGood =
            await _apiService.CreateParleyBet(parleyBetSlip);

        preBets.Clear();

        return parleyBetGood;
    }

    public GameSnapshotModel CreateGameSnapshot(GameDto gameDto) =>
        new()
        {
            Week = gameDto.Week,
            Date = gameDto.Date,
            AwayTeam = gameDto.AwayTeam,
            HomeTeam = gameDto.HomeTeam,
            PointSpread = Math.Round(Convert.ToDecimal(gameDto.PointSpread), 1),
            OverUnder = Math.Round(Convert.ToDecimal(gameDto.OverUnder), 1),
            AwayTeamMoneyLine = gameDto.AwayTeamMoneyLine,
            HomeTeamMoneyLine = gameDto.HomeTeamMoneyLine,
            PointSpreadAwayTeamMoneyLine = gameDto.PointSpreadAwayTeamMoneyLine,
            PointSpreadHomeTeamMoneyLine = gameDto.PointSpreadHomeTeamMoneyLine,
            ScoreID = gameDto.ScoreID,
            OverPayout = gameDto.OverPayout,
            UnderPayout = gameDto.UnderPayout
        };

    public decimal GetPayoutForTotalBetsParley(decimal totalParleyWager)
    {
        if (preBets.Count < 2) return 0;

        decimal totalDecimalOdds = 1;

        foreach (CreateBetModel createBetModel in preBets)
        {
            decimal decimalMoneyline =
                createBetModel.MoneylinePayout.ConvertMoneylinePayoutToDecimalFormat();

            totalDecimalOdds *= decimalMoneyline;
        }

        return totalPayoutForParley = totalParleyWager * totalDecimalOdds;
    }

    public BetSlipStateModel RemoveBetFromPreBetsList(CreateBetModel createBetModel)
    {
        preBets.Remove(createBetModel);
        return GetBetSlipState();
    }

    public decimal GetPayoutForTotalBetsSingles()
    {
        decimal total = 0;

        foreach (CreateBetModel createBetModel in preBets)
        {
            decimal betPayout = createBetModel.MoneylinePayout < 0 ?
                     createBetModel.BetAmount / ((decimal)createBetModel.MoneylinePayout * -1 / 100) + createBetModel.BetAmount
                     : ((decimal)createBetModel.MoneylinePayout / 100) * createBetModel.BetAmount;

            total += betPayout;
        }

        return total;
    }
}


