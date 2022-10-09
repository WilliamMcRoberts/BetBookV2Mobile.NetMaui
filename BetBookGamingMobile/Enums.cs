
using NetEscapades.EnumGenerators;

namespace BetBookGamingMobile;

[EnumExtensions]
public enum SeasonType
{
    PRE,
    REG,
    POST
}

[EnumExtensions]
public enum BetType
{
    POINTSPREAD,
    MONEYLINE,
    OVERUNDER
}

[EnumExtensions]
public enum ParleyBetSlipPayoutStatus
{
    UNPAID,
    PAID
}


[EnumExtensions]
public enum ParleyBetSlipStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER,
    PUSH
}

[EnumExtensions]
public enum SingleBetStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER,
    PUSH
}

[EnumExtensions]
public enum SingleBetPayoutStatus
{
    UNPAID,
    PAID
}

[EnumExtensions]
public enum SingleBetForParleyStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER,
    PUSH
}


public enum PageMode
{
    None,
    Menu,
    Navigate,
    Modal
}

public enum ContentDisplayMode
{
    NoNavigationBar,
    NavigationBar
}
