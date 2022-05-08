using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Ocdi.Alphaess;

public class AlphaESSClient
{
    private readonly HttpClient _client;

    public AlphaESSClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<ApiResult<AuthenticationData>?> Authenticate(string username, string password)
    {
        var result = await _client.PostAsJsonAsync("/api/Account/Login", new { username, password });
        return await result.Content.ReadFromJsonAsync<ApiResult<AuthenticationData>>();
    }

    public async Task<ApiResult<SystemData[]>?> SystemListAsync()
    {
        return await _client.GetFromJsonAsync<ApiResult<SystemData[]>>("/api/Account/GetCustomMenuESSlist");
    }
}

public class SystemData
{
    [JsonPropertyName("sys_sn")]
    public string SystemSerialNumber { get; set; }

    [JsonPropertyName("sys_name")]
    public string SystemName {get;set;}

    [JsonPropertyName("popv")]
    public float SolarPowerKW { get; set; }

    [JsonPropertyName("minv")]
    public string InverterModel { get; set; }
    [JsonPropertyName("poinv")]
    public float InverterPowerKW { get; set; }

    [JsonPropertyName("cobat")]
    public float BatteryCapacity { get; set; }

    [JsonPropertyName("mbat")]
    public string BatteryModel { get; set; }

    [JsonPropertyName("uscapacity")]
    public float UsuableCapacity { get; set; }

    [JsonPropertyName("ems_status")]
    public string SystemStatus { get; set; }

}