using Ocdi.Alphaess.Models;

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


var tz = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
DateTime Now() => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
var n = Now();

var today = DateOnly.FromDateTime(Now());
var peakStart = today.ToDateTime(new TimeOnly(15, 0), DateTimeKind.Local);


var chargeRatePer5 = 4.0;
var targetPercent = 100;

var systems = await client.SystemListAsync();
if (systems?.Data == null)
{
    Console.WriteLine("System list failed");
    return;
}
Console.WriteLine(JsonSerializer.Serialize(systems));
if (false)
{
    foreach (var sys in systems.Data)
    {
        var stats = await client.GetStatisticsByDay(sys.SystemSerialNumber, today);
        // element 120 is 10:00
        // element 180 is 15:00
        // let's calculate the charge rate over that time
        if (stats?.Data is { } d)
        {
            Console.WriteLine($"{"Time",6} {"BAT %",8} {"Chg R",8} {"Chg kw",8} {"PV kw",8} {"Load kw",8} {"Grid kw",8} {"Feed kw",8}");
            for (var i = 120; i <= 180; i++)
            {
                Console.WriteLine($"{d.Time[i],6} {d.Cbat[i],8:0.00} {d.Cbat[i] - d.Cbat[i - 1],8:0.00} {d.Ppv[i] - d.FeedIn[i] - d.HomePower[i] + d.GridCharge[i],8:0.00}{d.Ppv[i],8:0.00} {d.HomePower[i],8:0.00} {d.GridCharge[i],8:0.00} {d.FeedIn[i],8:0.00}");
            }
        }
    }
}
var currentSetting = (await client.GetCustomUseESSSetting())?.Data;

if (currentSetting == null)
{
    Console.WriteLine("ERROR: Can't get current charge settings");
    return;
}


while (true)
{

    var timeToPeak = peakStart - Now();
    // if this is less than zero, we've passed peak time so we should add a day
    if (timeToPeak.TotalSeconds < 0)
    {
        peakStart = peakStart.AddDays(1);
        continue;
    }

    foreach (var sys in systems.Data)
    {
        var lpd = await client.GetLastPowerDataBySN(sys.SystemSerialNumber);
        if (lpd?.Data is { } d)
        {
            var timeReqToCharge = TimeSpan.FromMinutes(1 + ((targetPercent - d.StateOfCharge) / chargeRatePer5 * 5));

            /* we have limited control over charging - if the duration is < 1hr, we turn off charging
             * if the time is > 1hr, we set the start time to be peak start - # of hours
             * as soon as we pass the timeReqToCharge, we adjust the start time to current hour
             * */

            if (timeReqToCharge.TotalMinutes < 60)
            {
                if (currentSetting.grid_charge == 1)
                {
                    currentSetting.grid_charge = 0;
                    currentSetting = await client.UpdateCustomUseESSSetting(currentSetting);
                }
            }
            else 
            {
                var startTime = peakStart.AddHours(-Math.Floor(timeReqToCharge.TotalMinutes / 60) - (timeReqToCharge > timeToPeak ? 1 : 0)).ToString("HH:mm");
                if (currentSetting.grid_charge == 0 || currentSetting.time_chaf1a != startTime)
                {
                    currentSetting.grid_charge = 1;
                    currentSetting.time_chaf1a = startTime;
                    currentSetting.time_chae1a = peakStart.ToString("HH:mm");
                    currentSetting = await client.UpdateCustomUseESSSetting(currentSetting);
                }
            }

            Console.WriteLine($"{"Serial",-20} {"SOC",4} {"PV",8} {"AC",8} {"BAT",8} {"Load",8} {"State",13} {"TTC",8} {"CST",8}");
            Console.WriteLine($"{sys.SystemSerialNumber,-20} {d} {timeReqToCharge,8:hh\\:mm} {timeToPeak - timeReqToCharge,8:hh\\:mm}");
        }
    }
    await Task.Delay(TimeSpan.FromSeconds(30));
}


