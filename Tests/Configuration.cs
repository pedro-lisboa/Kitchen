using Microsoft.Extensions.Configuration;
using System.IO;

namespace Tests
{
    public class Configuration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();

            return config;
        }

    }
}
