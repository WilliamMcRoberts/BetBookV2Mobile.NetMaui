using BetBookGamingMobile.Auth;
using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Helpers;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.ViewModels;
using Java.Lang;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Org.Apache.Http.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using StringBuilder = System.Text.StringBuilder;

namespace BetBookGamingMobile;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;

    public MainPage(MainViewModel viewModel, IAuthService authService, IMediator mediator)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _authService = authService;
        _mediator = mediator;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.Games.Count >= 1)
            return;

        _viewModel.Season = DateTime.Now.CalculateSeason();
        _viewModel.WeekNumber = _viewModel.Season.CalculateWeek(DateTime.Now);
        _viewModel.Title = _viewModel.Season == SeasonType.REG ? $"Regular Season Week {_viewModel.WeekNumber}"
             : _viewModel.Season == SeasonType.POST ? $"Post Season Week {_viewModel.WeekNumber}"
             : $"Pre Season Week {_viewModel.WeekNumber}";

        await _viewModel.GetGamesCommand.ExecuteAsync(null);
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        var result = await _authService.LoginAsync(CancellationToken.None);
        var token = result?.IdToken;
        // Access Token ??
        if (token is null) return;

        var handler = new JwtSecurityTokenHandler();
        var data = handler.ReadJwtToken(token);

        if (data is null) return;

        string objectId = data.Claims.FirstOrDefault(c => c.Type.Contains("oid"))?.Value;

        if (string.IsNullOrWhiteSpace(objectId) == false)
        {
            _viewModel.LoggedInUser = 
                await _mediator.Send(new GetUserByObjectIdQuery(objectId)) ?? new();

            string displayName = data.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
            string firstName = data.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value;
            string lastName = data.Claims.FirstOrDefault(c => c.Type.Contains("family_name"))?.Value;
            string jobTitle = data.Claims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value;
            string emailAddress = data.Claims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;

            bool isDirty = false;

            if (objectId.Equals(_viewModel.LoggedInUser.ObjectIdentifier) == false)
            {
                isDirty = true;
                _viewModel.LoggedInUser.ObjectIdentifier = objectId;
            }
            if (firstName.Equals(_viewModel.LoggedInUser.FirstName) == false)
            {
                isDirty = true;
                _viewModel.LoggedInUser.FirstName = firstName;
            }

            if (lastName.Equals(_viewModel.LoggedInUser.LastName) == false)
            {
                isDirty = true;
                _viewModel.LoggedInUser.LastName = lastName;
            }

            if (displayName.Equals(_viewModel.LoggedInUser.DisplayName) == false)
            {
                isDirty = true;
                _viewModel.LoggedInUser.DisplayName = displayName;
            }

            if (emailAddress.Equals(_viewModel.LoggedInUser.EmailAddress) == false)
            {
                isDirty = true;
                _viewModel.LoggedInUser.EmailAddress = emailAddress;
            }

            if (isDirty)
            {
                if (string.IsNullOrWhiteSpace(_viewModel.LoggedInUser.UserId))
                {
                    // New user recieves 10,000 in account
                    _viewModel.LoggedInUser.AccountBalance = 10000;

                    await _mediator.Send(new PostUserCommand(_viewModel.LoggedInUser));
                    return; 
                }

                await _mediator.Send(new PutUserCommand(_viewModel.LoggedInUser));
            }
        }
    }
}

