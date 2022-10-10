
namespace BetBookGamingMobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private bool isBusy = false;

    [ObservableProperty]
    private string loadingText = string.Empty;

    [ObservableProperty]
    private bool dataLoaded = false;

    [ObservableProperty]
    private bool isErrorState = false;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private string errorImage = string.Empty;

    [ObservableProperty]
    private bool isNotLoggedIn = true;

    public BaseViewModel() =>
        IsErrorState = false;

    protected void SetDataLoadingIndicators(bool isStarting = true)
    {
        if (isStarting)
        {
            IsBusy = true;
            DataLoaded = false;
            IsErrorState = false;
            ErrorMessage = "";
            ErrorImage = "";
        }
        else
        {
            LoadingText = "";
            IsBusy = false;
        }
    }
}
