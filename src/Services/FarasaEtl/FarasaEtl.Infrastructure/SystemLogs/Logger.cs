
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;


namespace FarasaEtl.Infrastructure.SystemLogs
{
    public static class Logger
    {
        public static ConfigurationManager CreateLog(this WebApplicationBuilder builder) 
        {
            var logSetting = builder.Configuration.GetSection(nameof(LogSetting)).Get<LogSetting>();
            var solutionName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName);
            logSetting ??= new LogSetting();

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(logSetting.MinimumLevel)
            .Enrich.FromLogContext()
            .WriteTo.Elasticsearch(new[] { new Uri(logSetting.Url), }, opts =>
            {
                opts.DataStream = new DataStreamName("logs", solutionName ?? "unknown", "backend");
                opts.BootstrapMethod = BootstrapMethod.Failure;
                opts.ConfigureChannel = channelOpts =>
                {
                    channelOpts.BufferOptions = new BufferOptions
                    {
                        ExportMaxRetries = 3,
                        
                    };
                };
            }, transport =>
            {
                if (logSetting.Insecure) transport.ServerCertificateValidationCallback((o, certificate, arg3, arg4) => { return true; });
                if (!string.IsNullOrEmpty(logSetting.ApiKey)) transport.Authentication(new ApiKey(logSetting.ApiKey)); // ApiKey
            })
            .CreateLogger();

            builder.Logging.AddProvider(new SerilogLoggerProvider(Log.Logger));

            return builder.Configuration;
        }

    }
}
