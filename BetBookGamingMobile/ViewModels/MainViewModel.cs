
namespace BetBookGamingMobile.ViewModels;

public partial class MainViewModel : AppBaseViewModel
{
    public event LoggedIn LoginComplete;
    public delegate void LoggedIn();
    private readonly IAuthService _authService;
    private readonly AuthenticationState _authState;
    
    [ObservableProperty]
    UserModel loggedInUser;

    public MainViewModel(IAuthService authService, AuthenticationState authState, IApiService apiService) :base(apiService)
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

        if (!string.IsNullOrEmpty(LoggedInUser.UserId))
            LoginComplete?.Invoke();
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

    public async Task LoadAndVerifyUserAsync(JwtSecurityToken data)
    {
        var authState = _authState.GetCurrentAuthenticationState();

        authState.ObjectId = data.Claims.FirstOrDefault(c => c.Type.Contains("oid"))?.Value;

        if (string.IsNullOrWhiteSpace(authState.ObjectId)) return;

        LoggedInUser = await _apiService.GetUserByObjectId(authState.ObjectId) ?? new();

        authState.DisplayName = data.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
        authState.FirstName = data.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value;
        authState.LastName = data.Claims.FirstOrDefault(c => c.Type.Contains("family_name"))?.Value;
        authState.EmailAddress = data.Claims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;
        string jobTitle = data.Claims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value;

        if(!string.IsNullOrEmpty(jobTitle)) authState.JobTitle = jobTitle;

        bool isDirty = false;

        if (!authState.ObjectId.Equals(LoggedInUser.ObjectIdentifier))
        {
            isDirty = true;
            LoggedInUser.ObjectIdentifier = authState.ObjectId;
        }

        if (!authState.FirstName.Equals(LoggedInUser.FirstName))
        {
            isDirty = true;
            LoggedInUser.FirstName = authState.FirstName;
        }

        if (!authState.LastName.Equals(LoggedInUser.LastName))
        {
            isDirty = true;
            LoggedInUser.LastName = authState.LastName;
        }

        if (!authState.DisplayName.Equals(LoggedInUser.DisplayName))
        {
            isDirty = true;
            LoggedInUser.DisplayName = authState.DisplayName;
        }

        if (!authState.EmailAddress.Equals(LoggedInUser.EmailAddress))
        {
            isDirty = true;
            LoggedInUser.EmailAddress = authState.EmailAddress;
        }

        if (LoggedInUser.AccountBalance <= 0)
        {
            isDirty = true;
            LoggedInUser.AccountBalance = 10000;
        }

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
