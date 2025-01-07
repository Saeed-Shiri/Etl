
using Hangfire.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FarasaEtl.Infrastructure.Data.Hangfire;
public class HangfireDbContext : DbContext
{
    public HangfireDbContext(DbContextOptions<HangfireDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        modelBuilder.OnHangfireModelCreating();
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(HangfireContext).Assembly);
    }
}
