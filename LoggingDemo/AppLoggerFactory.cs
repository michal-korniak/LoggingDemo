using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;
using MS = Microsoft.Extensions.Logging;

namespace ConsoleWithLogger
{
    internal class AppLoggerFactory : IDisposable
    {

        private readonly Logger _serilogLogger;
        private readonly ILoggerFactory _loggerFactory;

        public AppLoggerFactory()
        {
            string appName = "Demo";
            string envName = "Local";
            string elasticSearchUrl = "http://localhost:9200";
            string seqUrl = "http://localhost:5341/";

            _serilogLogger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithSpan()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Console(new CompactJsonFormatter())
                    .WriteTo.Seq(seqUrl)
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
                    {
                        IndexFormat = $"{appName}-{envName}-{DateTimeOffset.Now:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        OverwriteTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        TypeName = null,
                        BatchAction = ElasticOpType.Create,
                        InlineFields = true,
                    })
                    .CreateLogger();

            _loggerFactory = MS.LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(_serilogLogger);
            });
        }

        public MS.ILogger CreateLogger(string categoryName)
        {
            return _loggerFactory.CreateLogger(categoryName);
        }

        public MS.ILogger CreateLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }

        public void Dispose()
        {
            _serilogLogger?.Dispose();
            _loggerFactory?.Dispose();
        }
    }
}
