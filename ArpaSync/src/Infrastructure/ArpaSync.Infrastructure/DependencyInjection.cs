using ArpaSync.Application.Interfaces;
using ArpaSync.Infrastructure.Data;
using ArpaSync.Infrastructure.Repositories;
using ArpaSync.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace ArpaSync.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IOutboxEventRepository, OutboxEventRepository>();

        // HttpClient for Arpa API
        services.AddHttpClient<IArpaApiService, ArpaApiService>(client =>
        {
            var baseUrl = configuration["ArpaApi:BaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
            {
                client.BaseAddress = new Uri(baseUrl);
            }
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Serilog
        services.AddSerilog(config =>
        {
            config.ReadFrom.Configuration(configuration)
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                  .WriteTo.File("logs/arpa-sync-.txt", rollingInterval: RollingInterval.Day);
        });

        return services;
    }
}