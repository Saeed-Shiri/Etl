using FarasaEtl.Domain.Entities.Target;
using Microsoft.EntityFrameworkCore;

namespace FarasaEtl.Infrastructure.Data.Target
{
    public class MonitoringBaseInfoDbContext : DbContext
    {
        public MonitoringBaseInfoDbContext(DbContextOptions<MonitoringBaseInfoDbContext> options) : base(options) { }


        public DbSet<MarketMakingActivityLicense> MarketMakingActivityLicenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var entry in ChangeTracker.Entries<TargetBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        break;
                    default:
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
