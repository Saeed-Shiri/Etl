

using Hangfire;

namespace FarasaEtl.Application.Settings;
public abstract class JobBaseSetting
{
    public string CronExpression { get; set; } = Cron.Daily(23);
    public string JobId { get; set; }
}
