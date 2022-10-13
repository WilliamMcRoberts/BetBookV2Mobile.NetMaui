
namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : BasePage<GameDetailsViewModel>
{
    private readonly BetSlipState _betSlipState;

    public GameDetailsPage(GameDetailsViewModel viewModel, BetSlipState betSlipState) :base(viewModel)
    {
        InitializeComponent();
        _betSlipState = betSlipState;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        ViewModel.SetStateCommand.Execute(null);
    }
}
