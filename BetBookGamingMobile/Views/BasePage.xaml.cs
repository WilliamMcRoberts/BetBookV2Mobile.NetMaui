
namespace BetBookGamingMobile.Views;

public abstract class BasePage<TViewModel> : BasePage where TViewModel : BaseViewModel
{
    protected TViewModel ViewModel { get; }

    protected BasePage(TViewModel viewModel) : base(viewModel)
    {
        ViewModel = viewModel;
    }

    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}

public partial class BasePage : ContentPage
{
    protected BasePage(object viewModel = null)
    {
        BindingContext = viewModel;
    }
}

