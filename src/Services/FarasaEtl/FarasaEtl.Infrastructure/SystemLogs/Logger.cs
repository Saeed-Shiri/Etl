
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Serilog;


namespace FarasaEtl.Infrastructure.SystemLogs
{
    public static class Logger
    {
        public static ConfigurationManager CreateLog(this ConfigurationManager configuration) 
        {
            var logSetting = configuration.GetSection(nameof(LogSetting)).Get<LogSetting>();
            var solutionName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName);
            logSetting ??= new LogSetting();

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(logSetting.MinimumLevel)
            .Enrich.FromLogContext()
            .WriteTo.Elasticsearch(new[] { new Uri(logSetting.Url) }, opts =>
            {
                opts.DataStream = new DataStreamName("logs", solutionName ?? "unknown", "backend");
                opts.BootstrapMethod = BootstrapMethod.Failure;
                opts.ConfigureChannel = channelOpts =>
                {
                    channelOpts.BufferOptions = new BufferOptions
                    {
                    };
                };
            }, transport =>
            {
                if (logSetting.Insecure) transport.ServerCertificateValidationCallback((o, certificate, arg3, arg4) => { return true; });
                if (logSetting.ApiKey != null) transport.Authentication(new ApiKey(logSetting.ApiKey)); // ApiKey
            })
            .CreateLogger();

            return configuration;
        }
    }
}
