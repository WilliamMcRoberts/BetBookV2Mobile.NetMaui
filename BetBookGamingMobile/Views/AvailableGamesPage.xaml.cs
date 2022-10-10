
namespace BetBookGamingMobile.Views;

public partial class AvailableGamesPage : BasePage<AvailableGamesViewModel>
{
    public bool _isLoaded = false;

    public AvailableGamesPage(AvailableGamesViewModel viewModel) :base(viewModel)
    {
		InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!_isLoaded)
            await ViewModel.SetStateCommand.ExecuteAsync(null);
        _isLoaded = true;
    }
}