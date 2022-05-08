using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocdi.Alphaess;

var configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".alphaess.json");

AuthenticationData? data = null;

if (File.Exists(configFile)) {
    data = JsonSerializer.Deserialize<AuthenticationData>(File.ReadAllText(configFile));
}

var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(c => c.AddUserSecrets<Program>())
            .ConfigureServices(s => s.AddAlphaESSClient())
            .Build();

var client = host.Services.GetRequiredService<AlphaESSClient>();
var config = host.Services.GetRequiredService<IConfiguration>();

if (data?.AccessToken == null)
{

    Console.WriteLine(config["password"]);
    var res = await client.Authenticate(config["username"], config["password"]);
    if (res?.Data != null)
{
    var authJson = System.Text.Json.JsonSerializer.Serialize(res.Data);
    File.WriteAllText(configFile, authJson);
    Console.WriteLine(authJson);

}} else { 
    Console.WriteLine(data.AccessToken);

    client.
}



