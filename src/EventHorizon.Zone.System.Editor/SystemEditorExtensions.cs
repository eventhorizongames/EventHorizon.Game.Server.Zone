
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemEditorExtensions
    {
        public static IServiceCollection AddSystemEditor(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static void UseSystemEditor(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                
            }
        }
    }
}