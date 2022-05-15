using System.Net.Http.Json;

namespace Ocdi.Alphaess;

public class Credentials
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class AlphaESSClient
{
    private readonly HttpClient _client;

    public AccessData? AuthenticationData { get; protected set; }
    protected Credentials? Credentials { get; set; }


    public AlphaESSClient(HttpClient client) => _client = client;

    public async Task Init(Credentials credentials, AccessData? data)
    {
        AuthenticationData = data;
        Credentials = credentials;

        if (!ValidateTokenExpiry())
        {
            await Authenticate();
        }
    }

    public async Task<ApiResult<AccessData>?> Authenticate()
    {
        if (Credentials == null) throw new ArgumentNullException(nameof(Credentials), "Ensure Init is called before attempting to call this method");

        var result = await _client.PostAsJsonAsync("/api/Account/Login", new { username = Credentials.Username, password = Credentials.Password });
        var apiResult = await result.Content.ReadFromJsonAsync<ApiResult<AccessData>>();
        if (apiResult?.Data?.AccessToken != null)
            SetAuthentication(apiResult.Data);

        return apiResult;
    }

    private void SetAuthentication(AccessData? data)
    {
        if (data != null)
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", data.AccessToken);
    }

    public DateTime ParseTokenCreateTime(string tokenCreateTime)
    {
        if (tokenCreateTime.Contains('M')) // AM or PM
            return DateTime.ParseExact(tokenCreateTime, "MM/dd/yyyy hh:mm:ss tt", null);

        return DateTime.ParseExact(tokenCreateTime, "yyyy-MM-dd HH:mm:ss", null);
    }

    public bool ValidateTokenExpiry()
    {
        if (AuthenticationData == null || AuthenticationData.TokenCreateTime == null) return false;

        var tokenAge = DateTime.UtcNow - ParseTokenCreateTime( AuthenticationData.TokenCreateTime);
        if (tokenAge.TotalSeconds < AuthenticationData.ExpiresIn)
        {
            return true;
        }
        return false;
    }

    public async Task<ApiResult<SystemData[]>?> SystemListAsync() => await _client.GetFromJsonAsync<ApiResult<SystemData[]>>("/api/Account/GetCustomMenuESSlist");


}
