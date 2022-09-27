﻿
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.Services;

namespace BetBookGamingMobile.StateManagement;

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
    public readonly IGameService _gameService;
    public readonly IParleyBetSlipService _parleyBetSlipService;
    public readonly ISingleBetService _singleBetService;

    public BetSlipState(IGameService gameService,
                        ISingleBetService singleBetService,
                        IParleyBetSlipService parleyBetSlipService)
    {
        _gameService = gameService;
        _singleBetService = singleBetService;
        _parleyBetSlipService = parleyBetSlipService;
    }


    public void SelectOrRemoveWinnerAndGameForBet(string winner, GameDto game, BetType betType)
    {
        if (preBets.Contains(preBets.Where(b => b.Winner == winner && b.Game.ScoreID == game.ScoreID && b.BetType == betType)
                   .FirstOrDefault()!))
        {
            preBets.Remove(
            preBets.Where(b => b.Winner == winner && b.Game.ScoreID == game.ScoreID && b.BetType == betType)
                   .FirstOrDefault()!);

            conflictingBetsForParley = CheckForConflictingBets();
            return;
        }

        preBets.Add(new CreateBetModel
        {
            BetType = betType,
            BetAmount = 0,
            MoneylinePayout = GetMoneylinePayoutForBet(winner, game, betType),
            Game = game,
            Winner = winner,
            PointSpread = Math.Round(Convert.ToDecimal(game.PointSpread), 1),
            OverUnder = Math.Round(Convert.ToDecimal(game.OverUnder), 1),
        });

        conflictingBetsForParley = CheckForConflictingBets();
    }

    public ButtonColorStateModel GetButtonColorState(GameDto gameDto)
    {
        return new ButtonColorStateModel
        {
            ApColor = preBets.Contains(preBets.Where(b => b.Winner == gameDto.AwayTeam && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.POINTSPREAD).FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue,
            HpColor = preBets.Contains(preBets.Where(b => b.Winner == gameDto.HomeTeam && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.POINTSPREAD).FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue,
            AmColor = preBets.Contains(preBets.Where(b => b.Winner == gameDto.AwayTeam && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.MONEYLINE).FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue,
            HmColor = preBets.Contains(preBets.Where(b => b.Winner == gameDto.HomeTeam && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.MONEYLINE).FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue,
            OColor = preBets.Contains(preBets.Where(b => b.Winner == string.Concat("Over", gameDto.ScoreID.ToString()) && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.OVERUNDER).FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue,
            UColor = preBets.Contains(preBets.Where(b => b.Winner == string.Concat("Under", gameDto.ScoreID.ToString()) && b.Game.ScoreID == gameDto.ScoreID && b.BetType == BetType.OVERUNDER).FirstOrDefault()) ? Colors.DarkRed : Colors.DarkBlue
        };
    }

    public BetSlipStateModel GetBetSlipState()
    {
        var betSlipState = new BetSlipStateModel();

        foreach(var bet in preBets)
            betSlipState.BetsInBetSlip.Add(bet);

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

        GameDto[] gameCheckArray =
            await _gameService.GetGamesByWeek(season, week);

        foreach (CreateBetModel createBetModel in preBets)
        {
            if (createBetModel.BetAmount <= 0)
            {
                betAmountForSinglesBad = true;
                return false;
            }

            GameDto game = gameCheckArray.Where(g => g.ScoreID == createBetModel.Game.ScoreID)
                .FirstOrDefault()!;

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
                    Math.Round(CalculateSingleBetPayout(
                        createBetModel.BetAmount, createBetModel.MoneylinePayout), 2),

                BettorId = loggedInUser.UserId,
                BetType = createBetModel.BetType,
                BetAmount = createBetModel.BetAmount,
                SingleBetStatus = SingleBetStatus.IN_PROGRESS,
                SingleBetPayoutStatus = SingleBetPayoutStatus.UNPAID,
                GameSnapshot = CreateGameSnapshot(createBetModel.Game),
                PointsAfterSpread = CalculatePointsAfterSpread(
                            createBetModel.Game, createBetModel.Winner)
            };

            await _singleBetService.CreateSingleBet(singleBet);
        }

        preBets.Clear();
        return true;
    }

    public async Task<bool> OnSubmitBetsFromParleyBetSlip(UserModel loggedInUser, decimal parleyWagerAmount)
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

        GameDto[] gameCheckArray =
            await _gameService.GetGamesByWeek(season, week);

        var parleyBetSlip = new ParleyBetSlipModel();

        foreach (CreateBetModel createBetModel in preBets)
        {
            GameDto game = gameCheckArray.Where(g => g.ScoreID == createBetModel.Game.ScoreID)
                .FirstOrDefault()!;

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
                    CalculatePointsAfterSpread(createBetModel.Game, createBetModel.Winner),

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
            await _parleyBetSlipService.CreateParleyBet(parleyBetSlip);

        preBets.Clear();

        return parleyBetGood;
    }

    public GameSnapshotModel CreateGameSnapshot(GameDto gameDto) =>
        new GameSnapshotModel
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

    public decimal CalculatePointsAfterSpread(GameDto game, string chosenWinner) =>
         chosenWinner == game.HomeTeam ? 0 + (decimal)game.PointSpread!
            : 0 - (decimal)game.PointSpread!;

    public decimal CalculateSingleBetPayout(decimal betAmount, int moneylinePayout) =>
         moneylinePayout < 0 ? betAmount / ((decimal)moneylinePayout * -1 / 100) + betAmount
         : ((decimal)moneylinePayout / 100) * betAmount;

    public decimal GetPayoutForTotalBetsParley(decimal totalParleyWager)
    {
        if (preBets.Count < 2) return 0;

        decimal totalDecimalOdds = 1;

        foreach (CreateBetModel createBetModel in preBets)
        {
            decimal decimalMoneyline =
                ConvertMoneylinePayoutToDecimalFormat(createBetModel.MoneylinePayout);

            totalDecimalOdds *= decimalMoneyline;
        }

        return totalPayoutForParley = totalParleyWager * totalDecimalOdds;
    }

    public decimal ConvertMoneylinePayoutToDecimalFormat(int moneylinePayout) =>
         moneylinePayout < 0 ? (100 / (decimal)moneylinePayout * -1) + (decimal)1
         : ((decimal)moneylinePayout / 100) + 1;

    public void RemoveBetFromPreBetsList(CreateBetModel createBetModel)
    {
            preBets.Remove(createBetModel);
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

    public int GetMoneylinePayoutForBet(string winner, GameDto game, BetType betType) =>
        betType == BetType.POINTSPREAD ?
        (winner == game.AwayTeam ? (int)game.PointSpreadAwayTeamMoneyLine! : (int)game.PointSpreadHomeTeamMoneyLine!)
        : betType == BetType.OVERUNDER ? (winner[0] == 'O' ? (int)game.OverPayout! : (int)game.UnderPayout!)
        : (winner == game.AwayTeam ? (int)game.AwayTeamMoneyLine! : (int)game.HomeTeamMoneyLine!);

    public string GetWinnerSummary(CreateBetModel createBetModel) =>
        createBetModel.BetType == BetType.POINTSPREAD ? GetWinnerSummaryForPointSpread(createBetModel)
        : createBetModel.BetType == BetType.OVERUNDER ? GetWinnerSummaryForOverUnder(createBetModel)
        : createBetModel.Winner;

    public string GetWinnerSummaryForOverUnder(CreateBetModel createBetModel) =>
         createBetModel.Winner[0] == 'O' ? $"Over {createBetModel.Game.OverUnder}"
         : $"Under {createBetModel.Game.OverUnder}";

    public string GetWinnerSummaryForPointSpread(CreateBetModel createBetModel) =>
         createBetModel.Winner == createBetModel.Game.HomeTeam ?
         $"{createBetModel.Winner} {Convert.ToDecimal(createBetModel.Game.PointSpread):+#.0;-#.0}"
         : $"{createBetModel.Winner} {Convert.ToDecimal(createBetModel.Game.PointSpread):-#.0;+#.0;}";
}