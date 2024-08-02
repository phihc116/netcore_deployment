using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AppTelemetry
{
    public static class ServiceExtensions
    {
        public static void AddAppTelemetry(this IServiceCollection services, Action<TelemetryConfig> configureOptions)
        {
            var telemetryConfig = new TelemetryConfig();
            configureOptions(telemetryConfig);

            var serviceName = telemetryConfig.ServiceName;
            var serviceVersion = telemetryConfig.ServiceVersion;

            var otelServiceLink = Environment.GetEnvironmentVariable("OTEL_SERVICE_LINK");

            Console.WriteLine($"Telemetry configuration information: ${serviceName}: {otelServiceLink}");  

            services.AddOpenTelemetry()
             .ConfigureResource(resource => resource.AddService(
                 serviceName: serviceName,
                 serviceVersion: serviceVersion))
             .WithMetrics(metrics =>
             {
                 metrics
                 .AddMeter(serviceName)
                 .AddAspNetCoreInstrumentation()  
                 .AddHttpClientInstrumentation() 
                 .AddRuntimeInstrumentation()  
                 .AddProcessInstrumentation()  
                 .AddConsoleExporter()
                 .AddOtlpExporter(options =>
                 {
                     options.Endpoint = new Uri($"{otelServiceLink}:4318/v1/metrics");
                     options.Protocol = OtlpExportProtocol.HttpProtobuf;
                 });
             })
             .WithTracing(tracing =>
             {
                 tracing
                 .AddSource(serviceName)
                 .AddAspNetCoreInstrumentation()
                 .AddHttpClientInstrumentation()
                 .AddConsoleExporter()
                 .AddOtlpExporter()
                 .AddOtlpExporter(options =>
                 {
                     options.Endpoint = new Uri($"{otelServiceLink}:4317/v1/traces");
                     options.Protocol = OtlpExportProtocol.Grpc;
                 });
             })
             .WithLogging(logging =>
             {
                 logging.AddOtlpExporter(options =>
                 {
                     options.Endpoint = new Uri($"{otelServiceLink}:4318/v1/logs");
                     options.Protocol = OtlpExportProtocol.HttpProtobuf;
                 })
                .AddConsoleExporter();
             });
        }
    }
}
