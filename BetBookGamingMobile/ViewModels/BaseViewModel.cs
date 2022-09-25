

using CommunityToolkit.Mvvm.ComponentModel;

namespace BetBookGamingMobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;
}
