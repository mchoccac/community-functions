using System.Threading.Tasks;
using Community.Functions;
using Dapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace community_function2
{
    public static class UpdateSessionsCount
    {
        [FunctionName("UpdateSessionsCount")]
        public static async Task Run(
            [TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, 
            ILogger log,
            ExecutionContext context,
            System.Threading.CancellationToken token)
        {
            log.LogDebug("Updatting user sessions");

            var connectionString = Config.GetUsersDbConnectionString(context.FunctionAppDirectory);
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(token);

            var affectedRows = await connection.ExecuteAsync("UPDATE Users SET SessionsCount = SessionsCount + 1;");
            log.LogDebug("Updated {affectedRows} users.", affectedRows);
        }
    }
}
