﻿
namespace BetBookGamingMobile.GlobalStateManagement;

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

    public ButtonColorStateModel SelectOrRemoveWinnerAndGameForBet(
        string winner, GameDto game, BetType betType)
    {
        if (preBets.Contains(preBets.Where(b => b.Winner == winner && b.Game.ScoreID == game.ScoreID && b.BetType == betType)
                   .FirstOrDefault()!))
        {
            preBets.Remove(
            preBets.Where(b => b.Winner == winner && b.Game.ScoreID == game.ScoreID && b.BetType == betType)
                   .FirstOrDefault()!);

            conflictingBetsForParley = CheckForConflictingBets();
            return GetButtonColorState(game);
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
        return GetButtonColorState(game);
    }

    public (ButtonColorStateModel, ButtonTextStateModel) GetAllStates(GameDto gameDto) =>
        (GetButtonColorState(gameDto), gameDto.GetButtonTextState());

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

    public BetSlipStateModel GetBetSlipState()
    {
        var betSlipState = new BetSlipStateModel();

        betSlipState.BetsInBetSlip.AddRange(preBets);

        return betSlipState;
    }

    public bool CheckForConflictingBets()
    {
        foreach (var bet in preBets)
            if (preBets.Where(b => b.Game.ScoreID == bet.Game.ScoreID && b.BetType == bet.BetType).Count() > 1)
                return true;

        return false;
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
            if (bet.BetAmount is null || bet.BetAmount == 0)
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
                    Math.Round(bet.BetAmount.CalculateSingleBetPayout(bet.MoneylinePayout), 2),

                BettorId = loggedInUser.UserId,
                BetType = bet.BetType,
                BetAmount = bet.BetAmount,
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

            parleyBetSlip.SingleBetsForParleyList.Add(new SingleBetForParleyModel
            {
                WinnerChosen = bet.BetType == BetType.OVERUNDER ?
                               (bet.Winner[0] == 'O' ? "Over" : "Under")
                                : bet.Winner,

                PointsAfterSpread =
                    bet.Game.CalculatePointsAfterSpread(bet.Winner),

                BettorId = loggedInUser.UserId,
                BetType = bet.BetType,
                SingleBetForParleyStatus = SingleBetForParleyStatus.IN_PROGRESS,
                GameSnapshot = bet.Game.GetGameSnapshot()
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

    public decimal GetPayoutForTotalBetsParley(decimal totalParleyWager)
    {
        if (preBets.Count < 2) return 0;

        decimal totalDecimalOdds = 1;

        foreach (var bet in preBets)
        {
            decimal decimalMoneyline =
                bet.MoneylinePayout.ConvertMoneylinePayoutToDecimalFormat();

            totalDecimalOdds *= decimalMoneyline;
        }

        return totalParleyWager * totalDecimalOdds;
    }

    public BetSlipStateModel RemoveBetFromPreBetsList(CreateBetModel createBetModel)
    {
        preBets.Remove(createBetModel);
        return GetBetSlipState();
    }

    public decimal GetPayoutForTotalBetsSingles()
    {
        decimal total = 0;

        foreach (var bet in preBets.Where(b => b.BetAmount is not null))
        {
            decimal betPayout = bet.BetAmount.CalculateSingleBetPayout(bet.MoneylinePayout);
            total += betPayout;
        }

        return total;
    }
}


