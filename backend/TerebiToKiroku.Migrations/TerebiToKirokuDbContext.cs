using Microsoft.EntityFrameworkCore;
using TerebiToKiroku.Domain.Entities;

namespace TerebiToKiroku.Migrations
{
    public class TerebiToKirokuDbContext(DbContextOptions<TerebiToKirokuDbContext> options) : DbContext(options)
    {
        public DbSet<Video> Videos => Set<Video>();
        public DbSet<WatchSession> WatchSessions => Set<WatchSession>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }
    }
}
