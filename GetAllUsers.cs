using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Community.Functions
{
    public static class GetAllUsers
    {
        [FunctionName("GetAllUsers")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req,
            ILogger log,
            ExecutionContext context,
            System.Threading.CancellationToken token)
        {
            log.LogInformation("getting users...");

            var connectionString = Config.GetUsersDbConnectionString(context.FunctionAppDirectory);
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(token);

            var result = await connection.QueryAsync("SELECT * FROM Users");

            return new OkObjectResult(result);
        }
        
    }

    
}
