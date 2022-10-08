
namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : ContentPage
{
    private readonly GameDetailsViewModel _viewModel;

    public GameDetailsPage(GameDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        _viewModel.SetStateCommand.Execute(null);
    }
}