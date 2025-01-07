
using System.Data;
using FarasaEtl.Application.Interfaces;
using FarasaEtl.Application.Services.MarketMakingActivity;
using FarasaEtl.Application.Services.Article15Note1;
using FarasaEtl.Application.Settings;
using FarasaEtl.Domain.Repositories.Commands;
using FarasaEtl.Domain.Repositories.Queries;
using FarasaEtl.Infrastructure.Data.Hangfire;
using FarasaEtl.Infrastructure.Data.Target;
using FarasaEtl.Infrastructure.Jobs;
using Hangfire;
using Hangfire.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FarasaEtl.Infrastructure;
public static class DependenyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MarketMakingActivityLicenseSetting>()
            .BindConfiguration(nameof(MarketMakingActivityLicenseSetting))
            .Validate(option => !string.IsNullOrEmpty(option.JobId), "JobId cannot be null or empty")
            .ValidateOnStart();

        services.AddOptions<Article15Note1Setting>()
            .BindConfiguration(nameof(Article15Note1Setting))
            .Validate(option => !string.IsNullOrEmpty(option.JobId), "JobId cannot be null or empty")
            .ValidateOnStart();


        services.AddDbContextFactory<HangfireDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("HnagfireConnection")));
        services.AddHangfireJobManager()
            .AddRecurringJob<MarketMakingActivityLicenseService>()
            .AddRecurringJob<Article15Note1Service>();

        services.AddDbContext<MonitoringBaseInfoDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("MonitoringBaseInfoConnection"))
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine);
        });

        // Configure Dapper
        services.AddTransient<IDbConnection>(sp =>
            new SqlConnection(configuration.GetConnectionString("FarasaConnection")));

        services.Scan(x => x.FromAssemblyOf<MonitoringBaseInfoDbContext>()
        .AddClasses(classes => classes.Where(type => type is { IsClass: true, IsPublic: true, IsAbstract: false })
        .AssignableTo(typeof(ICommandRepository<>)))
        .AsImplementedInterfaces(interfaceType => interfaceType != typeof(ICommandRepository<>))
        .WithScopedLifetime());

        services.Scan(x => x.FromAssemblyOf<MonitoringBaseInfoDbContext>()
        .AddClasses(classes => classes.Where(type => type is { IsClass: true, IsPublic: true, IsAbstract: false })
        .AssignableTo(typeof(IQueryRepository<>)))
        .AsImplementedInterfaces(interfaceType => interfaceType != typeof(IQueryRepository<>))
        .WithScopedLifetime());

        return services;
    }


    public static JobConfiguration AddHangfireJobManager(this IServiceCollection services)
    {
        services.AddHangfire((serviceProvider, configuration) =>
            configuration.UseEFCoreStorage(
                () => serviceProvider.GetRequiredService<IDbContextFactory<HangfireDbContext>>().CreateDbContext(),
                new EFCoreStorageOptions
                {
                    CountersAggregationInterval = new TimeSpan(0, 5, 0),
                    DistributedLockTimeout = new TimeSpan(0, 10, 0),
                    JobExpirationCheckInterval = new TimeSpan(0, 30, 0),
                    QueuePollInterval = new TimeSpan(0, 0, 15),
                    Schema = string.Empty,
                    SlidingInvisibilityTimeout = new TimeSpan(0, 5, 0),
                }));

        services.AddHangfireServer();
        services.AddScoped<IJobManager, HangfireRecurringJobManager>();

        return new JobConfiguration(services);
    }

    public static IApplicationBuilder StartRecurringJobs(this IApplicationBuilder app)
    {
        var jobManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IJobManager>();
        jobManager.Start();
        return app;
    }
}
