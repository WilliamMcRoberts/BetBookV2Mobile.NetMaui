
namespace BetBookGamingMobile.Views;

public partial class BetSlipPage : BasePage<BetSlipViewModel>
{
    public BetSlipPage(BetSlipViewModel viewModel) :base(viewModel)
	{
		InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        ViewModel.SetStateCommand.Execute(null);
    }
}