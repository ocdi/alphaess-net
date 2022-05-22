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
if (systems?.Data == null)
{
    Console.WriteLine("System list failed");
    return;
}
Console.WriteLine(JsonSerializer.Serialize(systems));

while (true)
{

    foreach (var sys in systems.Data)
    {
        var lpd = await client.GetLastPowerDataBySN(sys.SystemSerialNumber);
        if (lpd?.Data is { } d)
        {
            Console.WriteLine($"{"Serial",-20} {"SOC",4} {"PV",8} {"BAT",8} {"Load",8}");
            Console.WriteLine($"{sys.SystemSerialNumber,-20} {d.soc,4:0.0} {d.pmeter_dc,8:0.00} {d.pbat,8:0.00} {d.pmeter_dc + d.pbat,8:0.0} {(d.pbat < 0 ? "Charing" : "Discharging")}");
        }
    }
    await Task.Delay(TimeSpan.FromSeconds(10));
}
