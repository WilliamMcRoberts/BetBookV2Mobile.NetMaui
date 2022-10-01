﻿

namespace BetBookGamingMobile.Dto;
#nullable enable
public class GamesByWeekAndSeasonDto
{
    public GameDto[]? GamesInWeek { get; set; }
}

public class GameDto
{
    public string GameKey { get; set; } = string.Empty;
    public int SeasonType { get; set; }
    public int Season { get; set; }
    public int Week { get; set; }
    public DateTime Date { get; set; }
    public string AwayTeam { get; set; } = string.Empty;
    public string HomeTeam { get; set; } = string.Empty;
    public object? AwayScore { get; set; }
    public object? HomeScore { get; set; }
    public string Channel { get; set; } = string.Empty;
    public float? PointSpread { get; set; }
    public float? OverUnder { get; set; }
    public object? Quarter { get; set; }
    public object? TimeRemaining { get; set; }
    public object? Possession { get; set; }
    public object? Down { get; set; }
    public string Distance { get; set; } = string.Empty;
    public object? YardLine { get; set; }
    public object? YardLineTerritory { get; set; }
    public object? RedZone { get; set; }
    public object? AwayScoreQuarter1 { get; set; }
    public object? AwayScoreQuarter2 { get; set; }
    public object? AwayScoreQuarter3 { get; set; }
    public object? AwayScoreQuarter4 { get; set; }
    public object? AwayScoreOvertime { get; set; }
    public object? HomeScoreQuarter1 { get; set; }
    public object? HomeScoreQuarter2 { get; set; }
    public object? HomeScoreQuarter3 { get; set; }
    public object? HomeScoreQuarter4 { get; set; }
    public object? HomeScoreOvertime { get; set; }
    public bool HasStarted { get; set; }
    public bool IsInProgress { get; set; }
    public bool IsOver { get; set; }
    public bool Has1stQuarterStarted { get; set; }
    public bool Has2ndQuarterStarted { get; set; }
    public bool Has3rdQuarterStarted { get; set; }
    public bool Has4thQuarterStarted { get; set; }
    public bool IsOvertime { get; set; }
    public object? DownAndDistance { get; set; }
    public string? QuarterDescription { get; set; }
    public int StadiumID { get; set; }
    public DateTime LastUpdated { get; set; }
    public object? GeoLat { get; set; }
    public object? GeoLong { get; set; }
    public object? ForecastTempLow { get; set; }
    public object? ForecastTempHigh { get; set; }
    public object? ForecastDescription { get; set; }
    public object? ForecastWindChill { get; set; }
    public object? ForecastWindSpeed { get; set; }
    public int? AwayTeamMoneyLine { get; set; }
    public int? HomeTeamMoneyLine { get; set; }
    public bool? Canceled { get; set; }
    public bool Closed { get; set; }
    public string LastPlay { get; set; } = string.Empty;
    public DateTime Day { get; set; }
    public DateTime DateTime { get; set; }
    public int AwayTeamID { get; set; }
    public int HomeTeamID { get; set; }
    public int GlobalGameID { get; set; }
    public int GlobalAwayTeamID { get; set; }
    public int GlobalHomeTeamID { get; set; }
    public int? PointSpreadAwayTeamMoneyLine { get; set; }
    public int? PointSpreadHomeTeamMoneyLine { get; set; }
    public int ScoreID { get; set; }
    public string Status { get; set; } = string.Empty;
    public object? GameEndDateTime { get; set; }
    public dynamic? HomeRotationNumber { get; set; }
    public dynamic? AwayRotationNumber { get; set; }
    public object? NeutralVenue { get; set; }
    public object? RefereeID { get; set; }
    public int? OverPayout { get; set; }
    public int? UnderPayout { get; set; }
    public object? HomeTimeouts { get; set; }
    public object? AwayTimeouts { get; set; }
    public DateTime DateTimeUTC { get; set; }
    public int? Attendance { get; set; }
    public StadiumDetailsDto? StadiumDetails { get; set; }
    public string? AwayTeamImage { get => $"{AwayTeam.ToLower()}.svg"; }
    public string? HomeTeamImage { get => $"{HomeTeam.ToLower()}.svg"; }
    public string? GameTitle { get => $"{AwayTeam} @ {HomeTeam} {DateOfGameOnly} {TimeOfGameOnly}"; }
    public string? DateOfGameOnly { get => Date.ToString("MM-dd"); }
    public string? TimeOfGameOnly { get => Date.ToString("hh:mm"); }
    public string? AwayTeamPointSpreadForDisplay { get => $"{PointSpread?.ToString("-#.0;+#.0;+0")}"; }
    public string? HomeTeamPointSpreadForDisplay { get => $"{PointSpread?.ToString("+#.0;-#.0;+0")}"; }
    public string? AwayTeamMoneylineForDisplay { get => $"{AwayTeamMoneyLine?.ToString("+;-#;")}"; }
    public string? HomeTeamMoneylineForDisplay { get => $"{HomeTeamMoneyLine?.ToString("+#;-#;")}"; }
    public string? OverUnderForOverDisplay { get => $"Over {OverUnder}"; }
    public string? OverUnderForUnderDisplay { get => $"Under {OverUnder}"; }
}

#nullable disable

public class StadiumDetailsDto
{
    public int StadiumID { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public int Capacity { get; set; }
    public string PlayingSurface { get; set; }
    public float GeoLat { get; set; }
    public float GeoLong { get; set; }
    public string Type { get; set; }
}
