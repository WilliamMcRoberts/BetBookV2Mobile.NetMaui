

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

        LoggedInUser = await _mediator.Send(
            new GetUserByObjectIdQuery(authState.ObjectId)) ?? new();

        authState.DisplayName = data.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
        authState.FirstName = data.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value;
        authState.LastName = data.Claims.FirstOrDefault(c => c.Type.Contains("family_name"))?.Value;
        authState.EmailAddress = data.Claims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;
        string jobTitle = data.Claims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value;
        if(!string.IsNullOrEmpty(jobTitle))
            authState.JobTitle = jobTitle;

        bool isDirty = false;

        (isDirty, LoggedInUser.ObjectIdentifier) = !authState.ObjectId.Equals(LoggedInUser.ObjectIdentifier) ?
            (true, authState.ObjectId) : (isDirty, LoggedInUser.ObjectIdentifier);

        (isDirty, LoggedInUser.FirstName) = !authState.FirstName.Equals(LoggedInUser.FirstName) ?
            (true, authState.FirstName) : (isDirty, LoggedInUser.FirstName);

        (isDirty, LoggedInUser.LastName) = !authState.LastName.Equals(LoggedInUser.LastName) ?
            (true, authState.LastName) : (isDirty, LoggedInUser.LastName);

        (isDirty, LoggedInUser.DisplayName) = !authState.DisplayName.Equals(LoggedInUser.DisplayName) ?
            (true, authState.DisplayName) : (isDirty, LoggedInUser.DisplayName);

        (isDirty, LoggedInUser.EmailAddress) = !authState.EmailAddress.Equals(LoggedInUser.EmailAddress) ?
            (true, authState.EmailAddress) : (isDirty, LoggedInUser.EmailAddress);

        (isDirty, LoggedInUser.AccountBalance) = LoggedInUser.AccountBalance <= 0 ? 
            (true, 10000) : (isDirty, LoggedInUser.AccountBalance);

        authState.LoggedInUser = LoggedInUser;

        await _mediator.Send(new SetCurrentAuthenticationStateCommand(authState));

        if (!isDirty)
            return;

        if (!string.IsNullOrWhiteSpace(LoggedInUser.UserId))
        {
            await _mediator.Send(new PutUserCommand(LoggedInUser));
            return;
        }

        await _mediator.Send(new PostUserCommand(LoggedInUser));
        await LoginAsync();
    }
}
