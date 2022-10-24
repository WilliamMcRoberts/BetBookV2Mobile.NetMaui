
namespace BetBookGamingMobile.Views;

public partial class MyBetsPage : BasePage<MyBetsViewModel>
{
    public MyBetsPage(MyBetsViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);


        SingleRadioButton.IsChecked = true;
        InProgressButton.Background = Brush.DarkRed;
        WinnersButton.Background = Brush.DarkBlue;
        LosersButton.Background = Brush.DarkBlue;
        PushButton.Background = Brush.DarkBlue;

        SetPageState();

		await ViewModel.SetStateCommand.ExecuteAsync(null);
    }

    private void BetStatusButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        button.Background = button.Background == Brush.DarkBlue ? Brush.DarkRed : Brush.DarkBlue;

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
                (InProgressButton.Background == Brush.DarkRed, WinnersButton.Background == Brush.DarkRed, 
                    LosersButton.Background == Brush.DarkRed, PushButton.Background == Brush.DarkRed);
            return;
        }

        (ParleyInProgressView.IsVisible, ParleyWinnersView.IsVisible, ParleyLosersView.IsVisible, ParleyPushView.IsVisible) =
            (InProgressButton.Background == Brush.DarkRed, WinnersButton.Background == Brush.DarkRed, 
                LosersButton.Background == Brush.DarkRed, PushButton.Background == Brush.DarkRed);
    }
}