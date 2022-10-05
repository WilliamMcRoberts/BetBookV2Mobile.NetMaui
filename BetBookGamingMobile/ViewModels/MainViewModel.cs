

using Android.Service.Autofill;
using BetBookGamingMobile.Auth;
using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using AuthenticationState = BetBookGamingMobile.StateManagement.AuthenticationState;

namespace BetBookGamingMobile.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IMediator _mediator;
    private readonly AuthenticationState _authenticationState;

    public MainViewModel(
        IMediator mediator, AuthenticationState authenticationState)
    {
        _mediator = mediator;
        _authenticationState = authenticationState;
    }
    
    [RelayCommand]
    async Task LoginAsync()
    {
        var data = await _mediator.Send(new GetAuthenticationClaimsQuery());

        await LoadAndVerifyUserAsync(data);

        if (string.IsNullOrWhiteSpace(LoggedInUser.UserId))
            return;

        IsLoggedIn = true;
        await GoToAvailableGamesPageAsync();
    }

    public async Task GoToAvailableGamesPageAsync() =>
        await Shell.Current.GoToAsync($"//AvailableGamesPage", true);

    public async Task LoadAndVerifyUserAsync(JwtSecurityToken data)
    {
        var currentAuthState = _authenticationState.GetCurrentAuthenticationState();

        currentAuthState.ObjectId = 
            data.Claims.FirstOrDefault(c => c.Type.Contains("oid"))?.Value;

        if (string.IsNullOrWhiteSpace(currentAuthState.ObjectId))
            return;

        LoggedInUser = await _mediator.Send(
            new GetUserByObjectIdQuery(currentAuthState.ObjectId)) ?? new();

        currentAuthState.DisplayName =
            data.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
        currentAuthState.FirstName =
            data.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value;
        currentAuthState.LastName =
            data.Claims.FirstOrDefault(c => c.Type.Contains("family_name"))?.Value;
        currentAuthState.EmailAddress =
            data.Claims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;
        currentAuthState.JobTitle =
            data.Claims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value;

        bool isDirty = false;

        (isDirty, LoggedInUser.ObjectIdentifier) = !currentAuthState.ObjectId.Equals(LoggedInUser.ObjectIdentifier) ?
            (true, currentAuthState.ObjectId) : (isDirty, LoggedInUser.ObjectIdentifier);

        (isDirty, LoggedInUser.FirstName) = !currentAuthState.FirstName.Equals(LoggedInUser.FirstName) ?
            (true, currentAuthState.FirstName) : (isDirty, LoggedInUser.FirstName);

        (isDirty, LoggedInUser.LastName) = !currentAuthState.LastName.Equals(LoggedInUser.LastName) ?
            (true, currentAuthState.LastName) : (isDirty, LoggedInUser.LastName);

        (isDirty, LoggedInUser.DisplayName) = !currentAuthState.DisplayName.Equals(LoggedInUser.DisplayName) ?
            (true, currentAuthState.DisplayName) : (isDirty, LoggedInUser.DisplayName);

        (isDirty, LoggedInUser.EmailAddress) = !currentAuthState.EmailAddress.Equals(LoggedInUser.EmailAddress) ?
            (true, currentAuthState.EmailAddress) : (isDirty, LoggedInUser.EmailAddress);

        (isDirty, LoggedInUser.AccountBalance) = LoggedInUser.AccountBalance <= 0 ? 
            (true, 10000) : (isDirty, LoggedInUser.AccountBalance);

        _authenticationState.CurrentAuthenticationState = currentAuthState;
        _authenticationState.CurrentAuthenticationState.LoggedInUser = LoggedInUser;

        if (!isDirty)
            return;

        if (!string.IsNullOrWhiteSpace(LoggedInUser.UserId))
        {
            await _mediator.Send(new PutUserCommand(LoggedInUser));
            return;
        }

        // New user recieves 10,000 in account
        LoggedInUser.AccountBalance = 10000;
        await _mediator.Send(new PostUserCommand(LoggedInUser));
        _authenticationState.CurrentAuthenticationState.LoggedInUser = LoggedInUser;
    }
}
