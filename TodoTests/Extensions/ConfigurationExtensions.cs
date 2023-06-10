using Microsoft.Extensions.Configuration;

namespace TodoTests.Tools;

public static class ConfigurationExtensions
{
    public static string? GetUriString(this IConfiguration configuration, string name)
    {
        return configuration?.GetSection("Uris")[name];
    }
}