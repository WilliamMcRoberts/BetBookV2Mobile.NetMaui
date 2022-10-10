
namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : BasePage<GameDetailsViewModel>
{
    public GameDetailsPage(GameDetailsViewModel viewModel) :base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        ViewModel.SetStateCommand.Execute(null);
    }
}