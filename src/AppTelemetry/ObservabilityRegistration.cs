using AppTelemetry.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace AppTelemetry;

public static class ObservabilityRegistration
{
    public static void AddObservability(this WebApplicationBuilder builder)
    {
        var observabilityOptions = builder
                                    .Configuration
                                    .GetSection(ObservabilityOptions.GetSectionName())
                                    .Get<ObservabilityOptions>();

        ArgumentNullException.ThrowIfNull(observabilityOptions, nameof(observabilityOptions));

        observabilityOptions.OltpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT")
                                            ?? "http://localhost";
 
        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(
                resource => resource
                            .AddService(serviceName: observabilityOptions.ServiceName, 
                                        serviceVersion: observabilityOptions.ServiceVersion))
            .AddMetrics(observabilityOptions)
            .AddTracing(observabilityOptions);

        builder.AddSerilog(observabilityOptions);
    }

    private static OpenTelemetryBuilder AddMetrics(this OpenTelemetryBuilder builder, ObservabilityOptions observabilityOptions)
    {
        builder.WithMetrics(metrics =>
        {
            metrics            
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation();

            metrics.AddOtlpExporter(options =>
            {                
                options.Endpoint = new Uri($"{observabilityOptions.OltpEndpoint}:4318/v1/metrics");
                options.Protocol = OtlpExportProtocol.HttpProtobuf;
            });
        });

        return builder;
    }

    private static OpenTelemetryBuilder AddTracing(this OpenTelemetryBuilder builder, ObservabilityOptions observabilityOptions)
    {
        builder.WithTracing(tracing =>
        {
            tracing
            .AddSource(observabilityOptions.ServiceName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();

            tracing.AddOtlpExporter(options =>
            {                
                options.Endpoint = new Uri($"{observabilityOptions.OltpEndpoint}:4317/v1/traces");
                options.Protocol = OtlpExportProtocol.Grpc;
            });
        });
        return builder;
    }

    private static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder, ObservabilityOptions observabilityOptions)
    {
        const string outputTemplate =
          "[{Level:w}]: {Timestamp:dd-MM-yyyy:HH:mm:ss} {MachineName} {EnvironmentName} {SourceContext} {Message}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.OpenTelemetry(options =>
            {                
                options.Endpoint = $"{observabilityOptions.OltpEndpoint}:4318/v1/logs";
                options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.HttpProtobuf;
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = observabilityOptions.ServiceName
                };
            })
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}