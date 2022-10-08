
namespace BetBookGamingMobile.Views;

public partial class BetSlipPage : ContentPage
{
    private readonly BetSlipViewModel _viewModel;

    public BetSlipPage(BetSlipViewModel viewModel)
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