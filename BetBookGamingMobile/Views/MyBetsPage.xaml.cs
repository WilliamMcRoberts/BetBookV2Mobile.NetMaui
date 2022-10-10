
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

		if (!_isLoaded)
			await ViewModel.SetStateCommand.ExecuteAsync(null);
		_isLoaded = true;
    }
}