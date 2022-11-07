
namespace BetBookGamingMobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = String.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(isNotBusy))]
    private bool isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(isNotRefreshing))]
    private bool isRefreshing;

    [ObservableProperty]
    private string loadingText = String.Empty;

    [ObservableProperty]
    private bool dataLoaded;

    [ObservableProperty]
    private bool isErrorState;

    [ObservableProperty]
    private string errorMessage = String.Empty;

    [ObservableProperty]
    private string errorImage = String.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(isNotLoggedIn))]
    private bool isLoggedIn;

    public bool isNotBusy => !IsBusy;

    public bool isNotRefreshing => !IsRefreshing;

    public bool isNotLoggedIn => !IsLoggedIn;

    public BaseViewModel() =>
        IsErrorState = false;
}
