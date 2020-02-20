using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Dapper;
using System.IO;
using Newtonsoft.Json;

namespace Community.Functions
{

    public static class CreateUser
    {
        [FunctionName("CreateUser")]
        public static async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req,
                ILogger log,
                ExecutionContext context,
                System.Threading.CancellationToken token)
        {
            log.LogInformation("saving user...");


            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var userData = JsonConvert.DeserializeObject(body);

            var connectionString = Config.GetUsersDbConnectionString(context.FunctionAppDirectory);
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(token);

            var userId = await connection.QuerySingleAsync<int>("INSERT INTO Users (DisplayName, Email) VALUES (@DisplayName, @Email); SELECT CAST(SCOPE_IDENTITY() as int);", userData);
            var user = await connection.QuerySingleAsync("SELECT TOP 1 * FROM Users WHERE Id = @userId", new { userId });

            return new CreatedResult($"/api/user/{userId}", user);
        }
        
    }

}