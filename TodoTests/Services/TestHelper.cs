using Microsoft.Extensions.Configuration;

namespace TodoTests.Services;

public static class TestHelper
{
    public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
    {
        return new ConfigurationBuilder()
            .SetBasePath(outputPath)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
    }
    
    
}