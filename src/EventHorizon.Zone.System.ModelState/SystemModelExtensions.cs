namespace EventHorizon.Zone.System.ModelState;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemModelExtensions
{
    public static IServiceCollection AddSystemModelState(
        this IServiceCollection services
    )
    {
        return services;
    }
    public static IApplicationBuilder UseSystemModelState(
        this IApplicationBuilder app
    )
    {
        return app;
    }
}
