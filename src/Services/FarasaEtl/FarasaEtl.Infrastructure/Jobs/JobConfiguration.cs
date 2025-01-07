using FarasaEtl.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FarasaEtl.Infrastructure.Jobs;
public class JobConfiguration
{
    private readonly IServiceCollection services;

    internal JobConfiguration(IServiceCollection services)
    {
        this.services = services;
    }

    public JobConfiguration AddRecurringJob<TJob>() where TJob : IRecurringJob
    {
        services.AddScoped(typeof(IRecurringJob), typeof(TJob));
        return this;
    }
}
