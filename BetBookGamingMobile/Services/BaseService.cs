
namespace BetBookGamingMobile.Services;

public class BaseService
{
    private HttpClient _httpClient;
    private readonly IConnectivity _connectivity;

    protected BaseService(IConnectivity connectivity)
    {
        _connectivity = connectivity;
    }

    protected void SetBaseURL(string apiBaseUrl)
    {
        _httpClient = new()
        {
            BaseAddress = new Uri(apiBaseUrl)
        };

        _httpClient.DefaultRequestHeaders.Accept.Clear();

        _httpClient.DefaultRequestHeaders.Accept
            .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        if (apiBaseUrl != Constants.GameServiceURL)
            AddHttpHeader("XApiKey", Constants.BetBookGamingApiKey);
    }

    protected void AddHttpHeader(string key, string value) =>
        _httpClient.DefaultRequestHeaders.Add(key, value);

    protected async Task<T> GetAsync<T>(string resource)
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert(
                "No internet.", "You do not have an internet connection", "OK");
        }

        return await _httpClient.GetFromJsonAsync<T>(resource);
    }

    //protected async Task<T> GetAsync<T>(string resource)
    //{
    //    //Get Json data (from Cache or Web)
    //    var json = await GetJsonAsync(resource);

    //    //Return the result
    //    return JsonSerializer.Deserialize<T>(json);
    //}

    //private async Task<string> GetJsonAsync(string resource)
    //{
    //    //No Cache Found, or Cached data was not required, or Internet connection is also available
    //    if (_connectivity.NetworkAccess != NetworkAccess.Internet)
    //        throw new InternetConnectionException();

    //    //Extract response from URI
    //    var response = await _httpClient.GetAsync(new Uri(_httpClient.BaseAddress, resource));

    //    //Check for valid response
    //    response.EnsureSuccessStatusCode();

    //    //Read Response
    //    string json = await response.Content.ReadAsStringAsync();

    //    //Return the result
    //    return json;
    //}


    protected async Task<HttpResponseMessage> PostAsync<T>(string uri, T payload)
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert(
                "No internet.", "You do not have an internet connection", "OK");
        }

        var dataToPost = JsonSerializer.Serialize(payload);

        var content = new StringContent(dataToPost, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(new Uri(_httpClient.BaseAddress, uri), content);

        response.EnsureSuccessStatusCode();

        return response;
    }

    protected async Task<HttpResponseMessage> PutAsync<T>(string uri, T payload)
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert(
                "No internet.", "You do not have an internet connection", "OK");
        }

        var dataToPut = JsonSerializer.Serialize(payload);

        var content = new StringContent(dataToPut, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(new Uri(_httpClient.BaseAddress, uri), content);

        response.EnsureSuccessStatusCode();

        return response;
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string uri)
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert(
                "No internet", "You do not have an internet connection", "OK");
        }

        HttpResponseMessage response = await _httpClient.DeleteAsync(new Uri(_httpClient.BaseAddress, uri));

        response.EnsureSuccessStatusCode();

        return response;
    }
}
