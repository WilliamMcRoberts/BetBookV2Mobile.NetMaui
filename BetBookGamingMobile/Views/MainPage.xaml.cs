namespace BetBookGamingMobile.Views;

public partial class MainPage : BasePage<MainViewModel>
{
    private ISnackbar _customSnackbar;

    public MainPage(MainViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        viewModel.LoginComplete += ViewModel_LoginComplete;
    }

    public async void ViewModel_LoginComplete()
    {
        // Create and show snackbar
        var options = new SnackbarOptions
        {
            BackgroundColor = Colors.DarkRed,
            TextColor = Colors.White,
            ActionButtonTextColor = Colors.White,
            CornerRadius = new CornerRadius(10),
            Font = Font.SystemFontOfSize(18)
        };

        _customSnackbar = Snackbar.Make(
            "You can navigate by using the menu located in the top left.",
            async () =>
            {
                await _customSnackbar.Dismiss();
                _customSnackbar.Dispose();
            },
            "GOT IT",
            TimeSpan.FromSeconds(60),
            options, anchor: SnackBarAnchor);

        await _customSnackbar.Show();
    }
}

