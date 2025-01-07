

using FarasaEtl.Application.Interfaces;
using FarasaEtl.Application.Settings;
using Hangfire;
using Microsoft.Extensions.Options;

namespace FarasaEtl.Application.Services.Article15Note1;
public class Article15Note1Service : IRecurringJob
{
    private readonly IOptionsMonitor<Article15Note1Setting> _options;

    public Article15Note1Service(IOptionsMonitor<Article15Note1Setting> options)
    {
        _options = options;
    }

    public string CronExpression => _options.CurrentValue.CronExpression;

    public string JobId => _options.CurrentValue.JobId;

    public async Task Execute(CancellationToken cancellationToken = default)
    {
         Console.WriteLine("Tabsare1");
    }
}
