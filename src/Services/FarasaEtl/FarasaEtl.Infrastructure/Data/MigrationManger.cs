using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarasaEtl.Infrastructure.Data.Hangfire;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FarasaEtl.Infrastructure.Data;
public static class MigrationManger
{
    public static async Task MigrateDatabase<TContext>(this IServiceCollection services, Func<TContext, IServiceProvider, Task> seeder) where TContext : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        await using var scope = serviceProvider.CreateAsyncScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbContext>>();
        var context = scope.ServiceProvider.GetService<TContext>();

        try
        {
            logger.LogInformation($"Started Db Migration: {typeof(TContext).Name} at {DateTime.Now}");

            await CallSeeder(seeder, context, scope.ServiceProvider);

            logger.LogInformation($"Migration Completed: {typeof(TContext).Name} at {DateTime.Now}");
        }
        catch (SqlException e)
        {

            logger.LogError(e, $"An error occurred while migrating db: {typeof(TContext).Name}");
        }
    }

    private static async Task CallSeeder<TContext>(Func<TContext, IServiceProvider, Task> seeder, TContext context, IServiceProvider services) where TContext : DbContext
    {
        if(context.GetType() == typeof(HangfireDbContext))
        {
            await context.Database.EnsureCreatedAsync();
        }
        
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await context.Database.MigrateAsync();
        }

        await seeder(context, services);
    }
}
