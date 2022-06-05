using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace Api
{
    public static class Program
    {
        public static void Main()
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder().Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) => builder.ConfigureKeyVault())
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Debug);
                    })
                .UseNLog();
        }

        public static IConfigurationBuilder ConfigureKeyVault(this IConfigurationBuilder configurationBuilder)
        {
            var configuration = configurationBuilder.Build();

            SecretClientOptions secretClientOptions = new SecretClientOptions();
            secretClientOptions.Retry.MaxRetries = 3;
            secretClientOptions.Retry.Mode = RetryMode.Fixed;
            secretClientOptions.Retry.Delay = TimeSpan.FromSeconds(1);
            secretClientOptions.Retry.NetworkTimeout = TimeSpan.FromSeconds(1);

            return configurationBuilder;
        }

        public static IConfigurationRoot ConfigureVariableProviders(this IConfigurationBuilder builder)
        {
            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .ConfigureKeyVault();

            return builder.Build();
        }
    }
}
