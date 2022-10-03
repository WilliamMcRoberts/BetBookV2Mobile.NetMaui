

using BetBookGamingMobile.Dto;

namespace BetBookGamingMobile.Models;

public class ButtonTextState
{
    public string ApText { get; set; }
    public string HpText { get; set; }
    public string AmText { get; set; }
    public string HmText { get; set; }
    public string OText { get; set; }
    public string UText { get; set; }

    // TODO - Organize State Getters

    //public ButtonTextState GetButtonTextState(GameDto gameDto) =>
    //new ButtonTextState
    //{
    //    ApText = $"{gameDto.AwayTeam} {gameDto.AwayTeamPointSpreadForDisplay} | {gameDto.PointSpreadAwayTeamMoneyLine}",
    //    HpText = $"{gameDto.HomeTeam} {gameDto.HomeTeamPointSpreadForDisplay} | {gameDto.PointSpreadHomeTeamMoneyLine}",
    //    AmText = $"{gameDto.AwayTeamMoneyLine}",
    //    HmText = $"{gameDto.HomeTeamMoneyLine}",
    //    OText = $"Over {gameDto.OverUnder} | {gameDto.OverPayout}",
    //    UText = $"Under {gameDto.OverUnder} | {gameDto.UnderPayout}"
    //};
}


