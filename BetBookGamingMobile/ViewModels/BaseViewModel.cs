
namespace BetBookGamingMobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private bool isBusy = false;

    [ObservableProperty]
    private bool isRefreshing;

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
    private bool isLoggedIn;

    [ObservableProperty]
    private bool isNotLoggedIn = true;

    public BaseViewModel() =>
        IsErrorState = false;
}
