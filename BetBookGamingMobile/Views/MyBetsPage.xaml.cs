
namespace BetBookGamingMobile.Views;

public partial class MyBetsPage : BasePage<MyBetsViewModel>
{
    public bool _isLoaded = false;

    public MyBetsPage(MyBetsViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        SetVisibilityState();
		if (!_isLoaded)
			await ViewModel.SetStateCommand.ExecuteAsync(null);
		_isLoaded = true;
    }

    private void SingleInProgressButton_Clicked(object sender, EventArgs e)
    {
        SingleInProgressView.IsVisible = true;
        SingleWinnersView.IsVisible = false;
        SingleLosersView.IsVisible = false;
        SinglePushView.IsVisible = false;
    }

    private void SingleWinnersButton_Clicked(object sender, EventArgs e)
    {
        SingleInProgressView.IsVisible = false;
        SingleWinnersView.IsVisible = true;
        SingleLosersView.IsVisible = false;
        SinglePushView.IsVisible = false;
    }

    private void SingleLosersButton_Clicked(object sender, EventArgs e)
    {
        SingleInProgressView.IsVisible = false;
        SingleWinnersView.IsVisible = false;
        SingleLosersView.IsVisible = true;
        SinglePushView.IsVisible = false;
    }

    private void SinglePushButton_Clicked(object sender, EventArgs e)
    {
        SingleInProgressView.IsVisible = false;
        SingleWinnersView.IsVisible = false;
        SingleLosersView.IsVisible = false;
        SinglePushView.IsVisible = true;
    }

    private void SetVisibilityState()
    {
        SingleInProgressView.IsVisible = true;
        SingleWinnersView.IsVisible = false;
        SingleLosersView.IsVisible = false;
        SinglePushView.IsVisible = false;
    }
}