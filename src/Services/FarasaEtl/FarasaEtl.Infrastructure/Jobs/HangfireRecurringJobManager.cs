using FarasaEtl.Application.Interfaces;
using Hangfire;

namespace FarasaEtl.Infrastructure.Jobs;
public class HangfireRecurringJobManager : IJobManager
{
    private readonly IRecurringJobManager _manager;
    private readonly IEnumerable<IRecurringJob> _jobs;

    public HangfireRecurringJobManager(IRecurringJobManager manager, IEnumerable<IRecurringJob> jobs)
    {
        _manager = manager;
        _jobs = jobs;
    }

    public void Start()
    {
        
        foreach (var job in _jobs)
        {
            _manager.AddOrUpdate(job.JobId, () => job.Execute(CancellationToken.None), job.CronExpression);
        }
    }
}
