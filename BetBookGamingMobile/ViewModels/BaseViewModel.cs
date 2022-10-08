
namespace BetBookGamingMobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{

    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    string title;

    [ObservableProperty]
    bool isLoggedIn;

    [ObservableProperty]
    UserModel loggedInUser;

    public bool IsNotLoggedIn => !isLoggedIn;
        public bool IsNotBusy => !IsBusy;

}
