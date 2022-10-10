
namespace BetBookGamingMobile.ViewModels;

public partial class MainViewModel : AppBaseViewModel
{
    private readonly IAuthService _authService;
    private readonly AuthenticationState _authState;
    UserModel loggedInUser;

    public MainViewModel(IAuthService authService, AuthenticationState authState, IApiService apiService) :base(apiService)
    {
        _authService = authService;
        _authState = authState;
    }
    
    [RelayCommand]
    async Task LoginAsync()
    {
        //var data = await _authService.GetAuthClaims();

        //await LoadAndVerifyUserAsync(data);

        //if (string.IsNullOrWhiteSpace(loggedInUser.UserId))
        //    return;

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

        loggedInUser = await _apiService.GetUserByObjectId(authState.ObjectId) ?? new();

        authState.DisplayName = data.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
        authState.FirstName = data.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value;
        authState.LastName = data.Claims.FirstOrDefault(c => c.Type.Contains("family_name"))?.Value;
        authState.EmailAddress = data.Claims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;
        string jobTitle = data.Claims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value;
        if(!string.IsNullOrEmpty(jobTitle))
            authState.JobTitle = jobTitle;

        bool isDirty = false;

        (isDirty, loggedInUser.ObjectIdentifier) = !authState.ObjectId.Equals(loggedInUser.ObjectIdentifier) ?
            (true, authState.ObjectId) : (isDirty, loggedInUser.ObjectIdentifier);

        (isDirty, loggedInUser.FirstName) = !authState.FirstName.Equals(loggedInUser.FirstName) ?
            (true, authState.FirstName) : (isDirty, loggedInUser.FirstName);

        (isDirty, loggedInUser.LastName) = !authState.LastName.Equals(loggedInUser.LastName) ?
            (true, authState.LastName) : (isDirty, loggedInUser.LastName);

        (isDirty, loggedInUser.DisplayName) = !authState.DisplayName.Equals(loggedInUser.DisplayName) ?
            (true, authState.DisplayName) : (isDirty, loggedInUser.DisplayName);

        (isDirty, loggedInUser.EmailAddress) = !authState.EmailAddress.Equals(loggedInUser.EmailAddress) ?
            (true, authState.EmailAddress) : (isDirty, loggedInUser.EmailAddress);

        (isDirty, loggedInUser.AccountBalance) = loggedInUser.AccountBalance <= 0 ? 
            (true, 10000) : (isDirty, loggedInUser.AccountBalance);

        _authState.CurrentAuthenticationState.LoggedInUser = loggedInUser;

        if (!isDirty)
            return;

        if (!string.IsNullOrWhiteSpace(loggedInUser.UserId))
        {
            await _apiService.UpdateUser(loggedInUser);
            return;
        }

        await _apiService.CreateUser(loggedInUser);
        loggedInUser = await _apiService.GetUserByObjectId(loggedInUser.ObjectIdentifier);
        _authState.CurrentAuthenticationState.LoggedInUser = loggedInUser;
    }
}
