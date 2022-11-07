
namespace BetBookGamingMobile.Helpers;

public static class ToastHelpers
{
    /// <summary>
    /// Shows number of bets in the preBets list
    /// </summary>
    /// <param name="betsInPreBets">int represents the number of bets in preBets list</param>
    /// <returns></returns>
    public static async Task ShowBetNumberToastAsync(this int betsInPreBets)
    {
        var toast = betsInPreBets != 1 ? Toast.Make($"You have {betsInPreBets} bets in your bet slip", textSize: 15)
        : Toast.Make($"You have 1 bet in your bet slip", textSize: 15);

        await toast.Show();
    }

    /// <summary>
    /// Shows wager confirmation toast if wager submission was successful
    /// </summary>
    /// <param name="betType">string represents the type of bet that was submitted</param>
    /// <returns></returns>
    public static async Task ShowWagerConfirmationToastAsync(this string betType)
    {
        var toast = Toast.Make($"Your {betType} wager was submitted!", textSize: 18);

        await toast.Show();
    }
}
