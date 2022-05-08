using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocdi.Alphaess;

var host = new HostBuilder().ConfigureServices(s => s.AddAlphaESSClient()).Build();
var client = host.Services.GetRequiredService<AlphaESSClient>();
var res = await client.Authenticate("USER", "PASS");

// See https://aka.ms/new-console-template for more information
Console.WriteLine(res);
