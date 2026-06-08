using Microsoft.EntityFrameworkCore;
using TerebiToKiroku.Domain.Customers;
using TerebiToKiroku.Domain.Payments;
using TerebiToKiroku.Domain.Products;
using TerebiToKiroku.Infrastructure.Processing.InternalCommands;

namespace TerebiToKiroku.Infrastructure.Database
{
    public class VideosContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }

        public DbSet<InternalCommand> InternalCommands { get; set; }


        public VideosContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VideosContext).Assembly);
        }
    }
}