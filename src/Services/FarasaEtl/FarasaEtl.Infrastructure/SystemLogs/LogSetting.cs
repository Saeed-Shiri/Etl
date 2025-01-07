

using Serilog.Events;

namespace FarasaEtl.Infrastructure.SystemLogs
{
    public class LogSetting
    {
        public string Url { get; init; } = "http://localhost:9200";
        public string? ApiKey { get; init; }
        public bool Insecure { get; init; }
        public LogEventLevel MinimumLevel { get; init; } = LogEventLevel.Debug;

    }
}
