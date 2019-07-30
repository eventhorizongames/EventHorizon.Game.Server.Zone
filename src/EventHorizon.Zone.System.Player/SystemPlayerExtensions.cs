
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemPlayerExtensions
    {
        public static IServiceCollection AddSystemPlayer(
            this IServiceCollection services
        )
        {
            return services.AddMediatR(
                typeof(SystemPlayerExtensions).Assembly
            );
        }
        public static void UseSystemPlayer(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                
            }
        }
    }
}