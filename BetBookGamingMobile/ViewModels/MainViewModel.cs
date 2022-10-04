

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
using System.IdentityModel.Tokens.Jwt;

namespace BetBookGamingMobile.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IMediator _mediator;
    private readonly AuthenticationState _authenticationState;

    public delegate bool GetBoolFromStringsDelegate(string userState, string authState);

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

        if (!string.IsNullOrWhiteSpace(_authenticationState.CurrentAuthenticationState.LoggedInUser.UserId))
            await GoToAvailableGamesPageAsync();
    }

    public async Task GoToAvailableGamesPageAsync() =>
        await Shell.Current.GoToAsync($"//AvailableGamesPage", true);

    public async Task LoadAndVerifyUserAsync(JwtSecurityToken data)
    {
        _authenticationState.CurrentAuthenticationState.ObjectId = 
            data.Claims.FirstOrDefault(c => c.Type.Contains("oid"))?.Value;

        if (string.IsNullOrWhiteSpace(_authenticationState.CurrentAuthenticationState.ObjectId))
            return;

        _authenticationState.CurrentAuthenticationState.LoggedInUser = await _mediator.Send(
            new GetUserByObjectIdQuery(_authenticationState.CurrentAuthenticationState.ObjectId)) ?? new();

        _authenticationState.CurrentAuthenticationState.DisplayName = 
            data.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;

        _authenticationState.CurrentAuthenticationState.FirstName = 
            data.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value;

        _authenticationState.CurrentAuthenticationState.LastName = 
            data.Claims.FirstOrDefault(c => c.Type.Contains("family_name"))?.Value;

        _authenticationState.CurrentAuthenticationState.EmailAddress = 
            data.Claims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;

        _authenticationState.CurrentAuthenticationState.JobTitle = 
            data.Claims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value;

        bool isDirty = false;

        (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.ObjectIdentifier) = 
            !_authenticationState.CurrentAuthenticationState.ObjectId.Equals(_authenticationState.CurrentAuthenticationState.LoggedInUser.ObjectIdentifier) ? 
            (true, _authenticationState.CurrentAuthenticationState.ObjectId) : (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.ObjectIdentifier);

        (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.FirstName) = 
            !_authenticationState.CurrentAuthenticationState.FirstName.Equals(_authenticationState.CurrentAuthenticationState.LoggedInUser.FirstName) ?
            (true, _authenticationState.CurrentAuthenticationState.FirstName) : (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.FirstName);

        (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.LastName) = 
            !_authenticationState.CurrentAuthenticationState.LastName.Equals(_authenticationState.CurrentAuthenticationState.LoggedInUser.LastName) ?
            (true, _authenticationState.CurrentAuthenticationState.LastName) : (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.LastName);

        (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.DisplayName) = 
            !_authenticationState.CurrentAuthenticationState.DisplayName.Equals(_authenticationState.CurrentAuthenticationState.LoggedInUser.DisplayName) ?
            (true, _authenticationState.CurrentAuthenticationState.DisplayName) : (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.DisplayName);

        (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.EmailAddress) = 
            !_authenticationState.CurrentAuthenticationState.EmailAddress.Equals(_authenticationState.CurrentAuthenticationState.LoggedInUser.EmailAddress) ?
            (true, _authenticationState.CurrentAuthenticationState.EmailAddress) : (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.EmailAddress);

        (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.AccountBalance) = 
            _authenticationState.CurrentAuthenticationState.LoggedInUser.AccountBalance <= 0 ? 
            (true, 10000) : (isDirty, _authenticationState.CurrentAuthenticationState.LoggedInUser.AccountBalance);

        if (!isDirty)
            return;

        if(!string.IsNullOrWhiteSpace(_authenticationState.CurrentAuthenticationState.LoggedInUser.UserId))
        {
            await _mediator.Send(new PutUserCommand(_authenticationState.CurrentAuthenticationState.LoggedInUser));
            return;
        }
            
        // New user recieves 10,000 in account
        _authenticationState.CurrentAuthenticationState.LoggedInUser.AccountBalance = 10000;
        await _mediator.Send(new PostUserCommand(_authenticationState.CurrentAuthenticationState.LoggedInUser));
    }
}
