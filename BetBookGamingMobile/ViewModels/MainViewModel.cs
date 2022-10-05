

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
    private UserModel loggedInUser;

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

        if (!string.IsNullOrWhiteSpace(loggedInUser.UserId))
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

        loggedInUser = await _mediator.Send(
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

        (isDirty, loggedInUser.ObjectIdentifier) = !currentAuthState.ObjectId.Equals(loggedInUser.ObjectIdentifier) ?
            (true, currentAuthState.ObjectId) : (isDirty, loggedInUser.ObjectIdentifier);

        (isDirty, loggedInUser.FirstName) = !currentAuthState.FirstName.Equals(loggedInUser.FirstName) ?
            (true, currentAuthState.FirstName) : (isDirty, loggedInUser.FirstName);

        (isDirty, loggedInUser.LastName) = !currentAuthState.LastName.Equals(loggedInUser.LastName) ?
            (true, currentAuthState.LastName) : (isDirty, loggedInUser.LastName);

        (isDirty, loggedInUser.DisplayName) = !currentAuthState.DisplayName.Equals(loggedInUser.DisplayName) ?
            (true, currentAuthState.DisplayName) : (isDirty, loggedInUser.DisplayName);

        (isDirty, loggedInUser.EmailAddress) = !currentAuthState.EmailAddress.Equals(loggedInUser.EmailAddress) ?
            (true, currentAuthState.EmailAddress) : (isDirty, loggedInUser.EmailAddress);

        (isDirty, loggedInUser.AccountBalance) = loggedInUser.AccountBalance <= 0 ? 
            (true, 10000) : (isDirty, loggedInUser.AccountBalance);

        _authenticationState.CurrentAuthenticationState = currentAuthState;
        _authenticationState.CurrentAuthenticationState.LoggedInUser = loggedInUser;

        if (string.IsNullOrWhiteSpace(loggedInUser.UserId))
        {
            // New user recieves 10,000 in account
            loggedInUser.AccountBalance = 10000;
            await _mediator.Send(new PostUserCommand(loggedInUser));
            _authenticationState.CurrentAuthenticationState.LoggedInUser = loggedInUser;
            return;
        }

        await _mediator.Send(new PutUserCommand(loggedInUser));
    }
}
