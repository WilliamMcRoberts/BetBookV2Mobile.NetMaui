

using BetBookGamingMobile.Auth;
using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using CommunityToolkit.Mvvm.ComponentModel;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace BetBookGamingMobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    string title;

    [ObservableProperty]
    public UserModel loggedInUser;

    [ObservableProperty]
    bool isLoggedIn;

    public bool IsNotLoggedIn => !isLoggedIn;
    public bool IsNotBusy => !IsBusy;

}
