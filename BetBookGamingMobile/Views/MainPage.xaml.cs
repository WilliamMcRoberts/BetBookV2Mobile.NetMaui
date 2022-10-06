using BetBookGamingMobile.Auth;
using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.GlobalStateManagement;
using BetBookGamingMobile.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace BetBookGamingMobile;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

