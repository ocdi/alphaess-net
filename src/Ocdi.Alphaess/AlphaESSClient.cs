using System.Net.Http.Json;

namespace Ocdi.Alphaess;

public class AlphaESSClient
{
    private readonly HttpClient _client;

    public AlphaESSClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> Authenticate(string username, string password) {
        var result = await _client.PostAsJsonAsync("/api/Account/Login", new { username, password});
        return await result.Content.ReadAsStringAsync();
        if (result.IsSuccessStatusCode) {
            // result.Content.ReadFromJsonAsync<
        }
    }
}

public class AuthenticateResult {

}