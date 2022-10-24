
namespace BetBookGamingMobile.Views;

public partial class AvailableGamesPage : BasePage<AvailableGamesViewModel>
{
    public AvailableGamesPage(AvailableGamesViewModel viewModel) :base(viewModel)
    {
		InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await ViewModel.SetStateCommand.ExecuteAsync(null);
    }
}