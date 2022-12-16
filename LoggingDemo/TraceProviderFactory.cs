using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace LoggingDemo
{
    internal class TracerProviderFactory
    {
        public static TracerProvider CreateTracerProvider(string serviceName)
        {
            var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource(serviceName)
            .SetResourceBuilder(ResourceBuilder
                .CreateDefault()
                .AddService(serviceName: serviceName))
            .AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"))
            .AddConsoleExporter()
            .Build();

            return tracerProvider;
        }
    }
}
