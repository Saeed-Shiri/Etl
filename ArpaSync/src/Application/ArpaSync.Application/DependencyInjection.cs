using ArpaSync.Application.Services;
using ArpaSync.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace ArpaSync.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Use Cases
        services.AddScoped<SyncCustomerWithArpaUseCase>();
        services.AddScoped<SyncOrderWithArpaUseCase>();
        services.AddScoped<SyncPaymentWithArpaUseCase>();

        // Background Services
        services.AddHostedService<OutboxEventProcessorService>();

        return services;
    }
}