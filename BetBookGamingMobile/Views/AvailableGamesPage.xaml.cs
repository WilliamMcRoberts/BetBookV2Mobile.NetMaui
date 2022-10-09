


namespace BetBookGamingMobile.Views;

public partial class AvailableGamesPage : ContentPage
{
    private readonly AvailableGamesViewModel _viewModel;
    public bool _isLoaded = false;

    public AvailableGamesPage(AvailableGamesViewModel viewModel)
    {
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!_isLoaded)
            await _viewModel.SetStateCommand.ExecuteAsync(null);
        _isLoaded = true;
    }
}