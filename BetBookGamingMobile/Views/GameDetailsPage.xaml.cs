
namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : BasePage<GameDetailsViewModel>
{
    public GameDetailsPage(GameDetailsViewModel viewModel) :base(viewModel)
    {
        InitializeComponent();
        viewModel.PointSpreadBetsChanged += ViewModel_PointSpreadBetSlipChanged;
        viewModel.MoneylineBetsChanged += ViewModel_MoneylineBetSlipChanged;
        viewModel.OverUnderBetsChanged += ViewModel_OverUnderBetSlipChanged;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        ViewModel.SetStateCommand.Execute(null);
    }

    private void ViewModel_PointSpreadBetSlipChanged(List<CreateBetModel> betList)
    {
        bool betAway = betList.Contains(betList.Where(b => b.Winner == ViewModel.GameDto.AwayTeam && b.Game.ScoreID == ViewModel.GameDto.ScoreID && b.BetType == BetType.POINTSPREAD).FirstOrDefault());
        bool betHome = betList.Contains(betList.Where(b => b.Winner == ViewModel.GameDto.HomeTeam && b.Game.ScoreID == ViewModel.GameDto.ScoreID && b.BetType == BetType.POINTSPREAD).FirstOrDefault());

        (ApButton.CustomBackground, HpButton.CustomBackground) =
            (betAway ? Brush.DarkRed : Brush.DarkBlue, betHome ? Brush.DarkRed : Brush.DarkBlue);
    }

    private void ViewModel_MoneylineBetSlipChanged(List<CreateBetModel> betList)
    {
        bool betAway = betList.Contains(betList.Where(b => b.Winner == ViewModel.GameDto.AwayTeam && b.Game.ScoreID == ViewModel.GameDto.ScoreID && b.BetType == BetType.MONEYLINE).FirstOrDefault());
        bool betHome = betList.Contains(betList.Where(b => b.Winner == ViewModel.GameDto.HomeTeam && b.Game.ScoreID == ViewModel.GameDto.ScoreID && b.BetType == BetType.MONEYLINE).FirstOrDefault());

        (AmButton.CustomBackground, HmButton.CustomBackground) =
            (betAway ? Brush.DarkRed : Brush.DarkBlue, betHome ? Brush.DarkRed : Brush.DarkBlue);
    }

    private void ViewModel_OverUnderBetSlipChanged(List<CreateBetModel> betList)
    {
        bool betOver = betList.Contains(betList.Where(b => b.Winner == string.Concat("Over", ViewModel.GameDto.ScoreID.ToString()) && b.Game.ScoreID == ViewModel.GameDto.ScoreID && b.BetType == BetType.OVERUNDER).FirstOrDefault());
        bool betUnder = betList.Contains(betList.Where(b => b.Winner == string.Concat("Under", ViewModel.GameDto.ScoreID.ToString()) && b.Game.ScoreID == ViewModel.GameDto.ScoreID && b.BetType == BetType.OVERUNDER).FirstOrDefault());

        (OButton.CustomBackground, UButton.CustomBackground) = 
            (betOver ? Brush.DarkRed : Brush.DarkBlue, betUnder ? Brush.DarkRed : Brush.DarkBlue);
    }
}
