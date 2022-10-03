using BetBookGamingMobile.Auth;
using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace BetBookGamingMobile;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;
    private readonly AuthState _authState;

    public MainPage(
        MainViewModel viewModel, IAuthService authService, IMediator mediator, AuthState authState)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _authService = authService;
        _mediator = mediator;
        _authState = authState;
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        //var data = await _authService.GetAuthClaims();

        //await LoadAndVerifyUser(data);

        //if (_authState.LoggedInUser is not null) 
            await _viewModel.GoToAvailableGamesPageAsync();
    }

    public async Task LoadAndVerifyUser(JwtSecurityToken data)
    {
        _authState.ObjectId = data.Claims.FirstOrDefault(c => c.Type.Contains("oid"))?.Value;

        if (string.IsNullOrWhiteSpace(_authState.ObjectId) == false)
        {
            _authState.LoggedInUser =
                await _mediator.Send(new GetUserByObjectIdQuery(_authState.ObjectId)) ?? new();

            _authState.DisplayName = data.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
            _authState.FirstName = data.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value;
            _authState.LastName = data.Claims.FirstOrDefault(c => c.Type.Contains("family_name"))?.Value;
            _authState.EmailAddress = data.Claims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;
            _authState.JobTitle = data.Claims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value;

            bool isDirty = false;

            if (!_authState.ObjectId.Equals(_authState.LoggedInUser.ObjectIdentifier))
            {
                isDirty = true;
                _authState.LoggedInUser.ObjectIdentifier = _authState.ObjectId;
            }

            if (!_authState.FirstName.Equals(_authState.LoggedInUser.FirstName))
            {
                isDirty = true;
                _authState.LoggedInUser.FirstName = _authState.FirstName;
            }

            if (!_authState.LastName.Equals(_authState.LoggedInUser.LastName))
            {
                isDirty = true;
                _authState.LoggedInUser.LastName = _authState.LastName;
            }

            if (!_authState.DisplayName.Equals(_authState.LoggedInUser.DisplayName))
            {
                isDirty = true;
                _authState.LoggedInUser.DisplayName = _authState.DisplayName;
            }

            if (!_authState.EmailAddress.Equals(_authState.LoggedInUser.EmailAddress))
            {
                isDirty = true;
                _authState.LoggedInUser.EmailAddress = _authState.EmailAddress;
            }

            if (_authState.LoggedInUser.AccountBalance <= 0)
            {
                isDirty = true;
                _authState.LoggedInUser.AccountBalance = 10000;
            }

            if (isDirty)
            {
                if (string.IsNullOrWhiteSpace(_authState.LoggedInUser.UserId))
                {
                    // New user recieves 10,000 in account
                    _authState.LoggedInUser.AccountBalance = 10000;

                    await _mediator.Send(new PostUserCommand(_authState.LoggedInUser));
                    return;
                }

                await _mediator.Send(new PutUserCommand(_authState.LoggedInUser));
            }
        }
    }
}

