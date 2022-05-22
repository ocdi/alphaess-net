using System.Net.Http.Json;

namespace Ocdi.Alphaess;

public class Credentials
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class AlphaESSClient
{
    public const string DateFormat = "yyyy-MM-dd";

    private readonly HttpClient _client;

    public AccessData? AuthenticationData { get; protected set; }
    protected Credentials? Credentials { get; set; }


    public AlphaESSClient(HttpClient client) => _client = client;

    public async Task Init(Credentials credentials, AccessData? data)
    {
        SetAuthentication(data);
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
        AuthenticationData = data;
        if (data != null)
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", data.AccessToken);
    }

    public static DateTime ParseTokenCreateTime(string tokenCreateTime)
    {
        if (tokenCreateTime.Contains('M')) // AM or PM
            return DateTime.ParseExact(tokenCreateTime, "MM/dd/yyyy hh:mm:ss tt", null);

        return DateTime.ParseExact(tokenCreateTime, "yyyy-MM-dd HH:mm:ss", null);
    }

    public bool ValidateTokenExpiry()
    {
        if (AuthenticationData == null || AuthenticationData.TokenCreateTime == null) return false;

        var tokenAge = DateTime.UtcNow - ParseTokenCreateTime(AuthenticationData.TokenCreateTime);
        if (tokenAge.TotalSeconds < AuthenticationData.ExpiresIn)
        {
            return true;
        }
        return false;
    }

    public async Task<ApiResult<SystemData[]>?> SystemListAsync() => await _client.GetFromJsonAsync<ApiResult<SystemData[]>>("/api/Account/GetCustomMenuESSlist");
    public async Task<ApiResult<StatisticsByDay>?> GetStatisticsByDay(string serialNumber, DateOnly requestedDay)
    {
        var result = await _client.PostAsJsonAsync("/api/Power/SticsByDay", new StatisticsByDayRequest { sn = serialNumber, sDate = DateTime.Today.ToString(DateFormat), userId = serialNumber, szDay = requestedDay.ToString(DateFormat) });
        var res = await result.Content.ReadAsStringAsync();
        return await result.Content.ReadFromJsonAsync<ApiResult<StatisticsByDay>>();
    }
    public async Task<ApiResult<LastPowerData>?> GetLastPowerDataBySN(string serialNumber)
    {
        var result = await _client.PostAsJsonAsync("/api/ESS/GetLastPowerDataBySN", new LastPowerRequest { sys_sn = serialNumber, noLoading = true });
        return await result.Content.ReadFromJsonAsync<ApiResult<LastPowerData>>();
    }
}
