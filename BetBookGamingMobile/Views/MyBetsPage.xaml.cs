
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

        SingleRadioButton.IsChecked = true;
        InProgressButton.BackgroundColor = Colors.DarkRed;

        SetPageState();

        if (!_isLoaded)
			await ViewModel.SetStateCommand.ExecuteAsync(null);
		_isLoaded = true;
    }

    private void BetStatusButton_Clicked(object sender, EventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);

        var button = (Button)sender;

        button.BackgroundColor = button.BackgroundColor == Colors.DarkBlue ? Colors.DarkRed : Colors.DarkBlue;

        SetPageState();
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        (SingleBetsVerticleStackLayout.IsVisible, ParleyBetsVerticleStackLayout.IsVisible) =
            (SingleRadioButton.IsChecked, ParleyRadioButton.IsChecked);

        SetPageState();
    }

    private void SetPageState()
    {
        if (SingleBetsVerticleStackLayout.IsVisible)
        {
            (SingleInProgressView.IsVisible, SingleWinnersView.IsVisible, SingleLosersView.IsVisible, SinglePushView.IsVisible) =
                (InProgressButton.BackgroundColor == Colors.DarkRed, WinnersButton.BackgroundColor == Colors.DarkRed, LosersButton.BackgroundColor == Colors.DarkRed, PushButton.BackgroundColor == Colors.DarkRed);
            return;
        }

        (ParleyInProgressView.IsVisible, ParleyWinnersView.IsVisible, ParleyLosersView.IsVisible, ParleyPushView.IsVisible) =
            (InProgressButton.BackgroundColor == Colors.DarkRed, WinnersButton.BackgroundColor == Colors.DarkRed, LosersButton.BackgroundColor == Colors.DarkRed, PushButton.BackgroundColor == Colors.DarkRed);
    }
}