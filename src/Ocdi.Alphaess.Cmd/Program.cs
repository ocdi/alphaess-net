var configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".alphaess.json");

AccessData? data = null;

if (File.Exists(configFile))
{
    data = JsonSerializer.Deserialize<AccessData>(File.ReadAllText(configFile));
}

var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(c => c.AddUserSecrets<Program>())
            .ConfigureServices(s => s.AddAlphaESSClient())
            .Build();

var client = host.Services.GetRequiredService<AlphaESSClient>();
var config = host.Services.GetRequiredService<IConfiguration>();

await client.Init(new Credentials { Username = config["username"], Password = config["password"] }, data);

// cache the credentials between runs
if (client.AuthenticationData?.AccessToken != null)
{
    var authJson = JsonSerializer.Serialize(client.AuthenticationData);
    File.WriteAllText(configFile, authJson);
    Console.WriteLine(authJson);
}



var systems = await client.SystemListAsync();

Console.WriteLine(JsonSerializer.Serialize(systems));