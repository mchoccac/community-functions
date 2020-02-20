using Microsoft.Extensions.Configuration;

namespace Community.Functions
{
    internal static class Config {
        internal static IConfiguration GetConfiguration(string functionAppDirectory) => 
            new ConfigurationBuilder()
                .SetBasePath(functionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

        internal static string GetUsersDbConnectionString(string functionAppDirectory) => 
            GetConfiguration(functionAppDirectory)
            .GetConnectionString("UsersDb");
    }
}