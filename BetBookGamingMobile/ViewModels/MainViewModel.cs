

using BetBookGamingMobile.Auth;
using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Org.Apache.Http.Authentication;
using System.IdentityModel.Tokens.Jwt;
using AuthenticationState = BetBookGamingMobile.GlobalStateManagement.AuthenticationState;

namespace BetBookGamingMobile.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IMediator _mediator;

    public MainViewModel(
        IMediator mediator)
    {
        _mediator = mediator;
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
        var authState = await _mediator.Send(new GetCurrentAuthenticationStateQuery());

        authState.ObjectId = 
            data.Claims.FirstOrDefault(c => c.Type.Contains("oid"))?.Value;

        if (string.IsNullOrWhiteSpace(authState.ObjectId))
            return;

        LoggedInUser = authState.LoggedInUser = await _mediator.Send(
            new GetUserByObjectIdQuery(authState.ObjectId)) ?? new();

        authState.DisplayName = data.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
        authState.FirstName = data.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value;
        authState.LastName = data.Claims.FirstOrDefault(c => c.Type.Contains("family_name"))?.Value;
        authState.EmailAddress = data.Claims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;
        authState.JobTitle = data.Claims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value;

        bool isDirty = false;

        (isDirty, authState.LoggedInUser.ObjectIdentifier) = !authState.ObjectId.Equals(authState.LoggedInUser.ObjectIdentifier) ?
            (true, authState.ObjectId) : (isDirty, authState.LoggedInUser.ObjectIdentifier);

        (isDirty, authState.LoggedInUser.FirstName) = !authState.FirstName.Equals(authState.LoggedInUser.FirstName) ?
            (true, authState.FirstName) : (isDirty, authState.LoggedInUser.FirstName);

        (isDirty, authState.LoggedInUser.LastName) = !authState.LastName.Equals(authState.LoggedInUser.LastName) ?
            (true, authState.LastName) : (isDirty, authState.LoggedInUser.LastName);

        (isDirty, authState.LoggedInUser.DisplayName) = !authState.DisplayName.Equals(authState.LoggedInUser.DisplayName) ?
            (true, authState.DisplayName) : (isDirty, authState.LoggedInUser.DisplayName);

        (isDirty, authState.LoggedInUser.EmailAddress) = !authState.EmailAddress.Equals(authState.LoggedInUser.EmailAddress) ?
            (true, authState.EmailAddress) : (isDirty, authState.LoggedInUser.EmailAddress);

        (isDirty, authState.LoggedInUser.AccountBalance) = authState.LoggedInUser.AccountBalance <= 0 ? 
            (true, 10000) : (isDirty, authState.LoggedInUser.AccountBalance);

        await _mediator.Send(new SetCurrentAuthenticationStateCommand(authState));

        if (!isDirty)
            return;

        if (!string.IsNullOrWhiteSpace(authState.LoggedInUser.UserId))
        {
            await _mediator.Send(new PutUserCommand(authState.LoggedInUser));
            return;
        }

        await _mediator.Send(new PostUserCommand(authState.LoggedInUser));
    }
}
