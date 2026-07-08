using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TerebiToKiroku.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TerebiToKirokuDbContext>
    {
        public TerebiToKirokuDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connStr = config.GetConnectionString("Default") ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");

            var builder = new DbContextOptionsBuilder<TerebiToKirokuDbContext>();
            builder.UseNpgsql(connStr);

            return new TerebiToKirokuDbContext(builder.Options);
        }
    }
}
