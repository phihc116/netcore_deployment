using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AppTelemetry
{
    public static class Extensions
    {
        public static void ConfigureBuilderDefault(this WebApplicationBuilder builder)
        {
            builder.AddObservability(); 
            builder.ConfigureHeathCheck();
        }
 
        public static void ConfigureHeathCheck(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks();
        }

        public static void ConfigureDefautApp(this IApplicationBuilder applicationBuilder)
        {
            //applicationBuilder.UseSerilogRequestLogging();
        }

        public static void ConfigureMapEndpointDefault(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapHealthChecks("/heathz");
        }
    }
}
