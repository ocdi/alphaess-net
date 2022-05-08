using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ocdi.Alphaess;

public class AlphaESSClientOptions
{
    public Uri BaseUri { get; set; } = new Uri("https://www.alphaess.com/api");
}
public static class Extensions
{
    public static IServiceCollection AddAlphaESSClient(this IServiceCollection services, Action<AlphaESSClientOptions>? config = null)
    {
        services.Configure<AlphaESSClientOptions>(config ?? (c => {}));
        services.AddHttpClient<AlphaESSClient>((sp, c) =>
        {
            var options = sp.GetRequiredService<IOptions<AlphaESSClientOptions>>();
            c.BaseAddress = options.Value.BaseUri;
        });
        return services;
    }
}