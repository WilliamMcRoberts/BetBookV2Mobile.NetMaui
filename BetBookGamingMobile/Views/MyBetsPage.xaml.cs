
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

        if (SingleBetsVerticleStackLayout.IsVisible)
        {
            switch (button.Text)
            {
                case "Active":
                    SingleInProgressView.IsVisible = InProgressButton.BackgroundColor == Colors.DarkRed;
                    break;
                case "Winners":
                    SingleWinnersView.IsVisible = WinnersButton.BackgroundColor == Colors.DarkRed;
                    break;
                case "Losers":
                    SingleLosersView.IsVisible = LosersButton.BackgroundColor == Colors.DarkRed;
                    break;
                case "Push":
                    SinglePushView.IsVisible = PushButton.BackgroundColor == Colors.DarkRed;
                    break;
            }
            return;
        }

        switch (button.Text)
        {
            case "Active":
                ParleyInProgressView.IsVisible = InProgressButton.BackgroundColor == Colors.DarkRed;
                break;
            case "Winners":
                ParleyWinnersView.IsVisible = WinnersButton.BackgroundColor == Colors.DarkRed;
                break;
            case "Losers":
                ParleyLosersView.IsVisible = LosersButton.BackgroundColor == Colors.DarkRed;
                break;
            case "Push":
                ParleyPushView.IsVisible = PushButton.BackgroundColor == Colors.DarkRed;
                break;
        }
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var radioButton = (RadioButton)sender;
        (SingleBetsVerticleStackLayout.IsVisible, ParleyBetsVerticleStackLayout.IsVisible) =
            radioButton.Content.ToString() == "Single Bets" ? (true, false) : (false, true);
    }

    private void SetPageState()
    {
        SingleRadioButton.IsChecked = true;
        (InProgressButton.BackgroundColor, WinnersButton.BackgroundColor, LosersButton.BackgroundColor, PushButton.BackgroundColor) =
            (Colors.DarkRed, Colors.DarkBlue, Colors.DarkBlue, Colors.DarkBlue);
        (SingleBetsVerticleStackLayout.IsVisible, ParleyBetsVerticleStackLayout.IsVisible) = (true, false);
        (SingleInProgressView.IsVisible, SingleWinnersView.IsVisible, SingleLosersView.IsVisible, SinglePushView.IsVisible) =
            (true, false, false, false);
        (ParleyInProgressView.IsVisible, ParleyWinnersView.IsVisible, ParleyLosersView.IsVisible, ParleyPushView.IsVisible) =
            (false, false, false, false);

    }
}