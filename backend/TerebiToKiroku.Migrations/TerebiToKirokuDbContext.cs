using Microsoft.EntityFrameworkCore;
using TerebiToKiroku.Domain.Entities;

namespace TerebiToKiroku.Migrations
{
    public class TerebiToKirokuDbContext(DbContextOptions<TerebiToKirokuDbContext> options) : DbContext(options)
    {
        public DbSet<Video> Videos => Set<Video>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Video>(e =>
            {
                e.ToTable("videos");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
            });
        }
    }
}
