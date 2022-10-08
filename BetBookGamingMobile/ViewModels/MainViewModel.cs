
using Org.Apache.Http.Authentication;

namespace BetBookGamingMobile.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly AuthenticationState _authState;

    public MainViewModel(IUserService userService, IAuthService authService, AuthenticationState authState)
    {
        _userService = userService;
        _authService = authService;
        _authState = authState;
    }
    
    [RelayCommand]
    async Task LoginAsync()
    {
        var data = await _authService.GetAuthClaims();

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
        var authState = _authState.GetCurrentAuthenticationState();

        authState.ObjectId = 
            data.Claims.FirstOrDefault(c => c.Type.Contains("oid"))?.Value;

        if (string.IsNullOrWhiteSpace(authState.ObjectId))
            return;

        LoggedInUser = await _userService.GetUserByObjectId(authState.ObjectId) ?? new();

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

        _authState.CurrentAuthenticationState.LoggedInUser = LoggedInUser;

        if (!isDirty)
            return;

        if (!string.IsNullOrWhiteSpace(LoggedInUser.UserId))
        {
            await _userService.UpdateUser(LoggedInUser);
            return;
        }

        await _userService.CreateUser(LoggedInUser);
        LoggedInUser = await _userService.GetUserByObjectId(LoggedInUser.ObjectIdentifier);
    }
}
