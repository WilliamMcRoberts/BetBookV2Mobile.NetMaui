

using CommunityToolkit.Mvvm.ComponentModel;
using IntelliJ.Lang.Annotations;

namespace BetBookGamingMobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;
}
