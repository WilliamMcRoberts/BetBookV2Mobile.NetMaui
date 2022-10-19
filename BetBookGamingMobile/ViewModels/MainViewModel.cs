
namespace BetBookGamingMobile.ViewModels;

public partial class MainViewModel : AppBaseViewModel
{
    private readonly IAuthService _authService;
    private readonly AuthenticationState _authState;

    [ObservableProperty]
    UserModel loggedInUser;

    public MainViewModel(
        IAuthService authService,AuthenticationState authState, IApiService apiService) :base(apiService)
    {
        _authService = authService;
        _authState = authState;
    }
    
    [RelayCommand]
    async Task LoginAsync()
    {
        var data = await _authService.GetAuthClaims();

        if (IsBusy) return;

        IsBusy = true;

        try
        {
            await LoadAndVerifyUserAsync(data);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }

        IsLoggedIn = !string.IsNullOrWhiteSpace(LoggedInUser.UserId);
        IsNotLoggedIn = !IsLoggedIn;

        if(IsLoggedIn)
            await GoToAvailableGamesPageAsync();
    }

    [RelayCommand]
    void Logout()
    {
        var authState = _authState.GetCurrentAuthenticationState();
        authState.LoggedInUser = null;
        authState.FirstName = "";
        authState.DisplayName = "";
        authState.LastName = "";
        authState.EmailAddress = "";
        authState.ObjectId = "";
        authState.JobTitle = "";
        LoggedInUser = null;
    }

    public async Task GoToAvailableGamesPageAsync() =>
        await Shell.Current.GoToAsync("//AvailableGamesPage");

    public async Task LoadAndVerifyUserAsync(JwtSecurityToken data)
    {
        var authState = _authState.GetCurrentAuthenticationState();

        authState.ObjectId = 
            data.Claims.FirstOrDefault(c => c.Type.Contains("oid"))?.Value;

        if (string.IsNullOrWhiteSpace(authState.ObjectId)) return;

        LoggedInUser = await _apiService.GetUserByObjectId(authState.ObjectId) ?? new();

        authState.DisplayName = data.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
        authState.FirstName = data.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value;
        authState.LastName = data.Claims.FirstOrDefault(c => c.Type.Contains("family_name"))?.Value;
        authState.EmailAddress = data.Claims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;
        string jobTitle = data.Claims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value;

        if(!string.IsNullOrEmpty(jobTitle)) authState.JobTitle = jobTitle;

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

        if (!isDirty) return;

        if (!string.IsNullOrWhiteSpace(LoggedInUser.UserId))
        {
            await _apiService.UpdateUser(LoggedInUser);
            return;
        }

        await _apiService.CreateUser(LoggedInUser);

        if (string.IsNullOrEmpty(LoggedInUser.ObjectIdentifier)) await LoginAsync();
    }
}
