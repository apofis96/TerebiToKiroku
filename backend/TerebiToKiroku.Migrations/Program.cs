using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TerebiToKiroku.Migrations
{
    internal class Program
    {
        static void Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connStr = config.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");

            var options = new DbContextOptionsBuilder<TerebiToKirokuDbContext>()
                .UseNpgsql(connStr)
                .Options;

            using var db = new TerebiToKirokuDbContext(options);
            Console.WriteLine("Applying migrations...");
            db.Database.Migrate();
            Console.WriteLine("Done.");
        }
    }
}
