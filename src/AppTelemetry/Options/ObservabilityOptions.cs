namespace AppTelemetry.Options
{
    internal sealed class ObservabilityOptions
    {
        public static string GetSectionName() => "Observability";        

        public required string ServiceName { get; set; }

        public required string ServiceVersion { get; set; }

        public string? OltpEndpoint { get; set; }
    }
}
