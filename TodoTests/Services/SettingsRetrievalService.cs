using Microsoft.Extensions.Configuration;

namespace TodoTests.Services;



public static class SettingsRetrievalService
{
    private static IConfiguration _configuration;

    public static string TodoDbConnectionString { get; set; } = "default connection string";

    public static void SettingsRetrievalServiceConfigure()
    {
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("C:\\Projects\\TodoTests\\TodoTests\\appsettings.json", optional: true, reloadOnChange: true);

        _configuration = configBuilder.Build();
        MapConnectionStrings();
    }

    private static void MapConnectionStrings()
    {
        TodoDbConnectionString = _configuration.GetConnectionString("TododbConnectionString");
    }
}