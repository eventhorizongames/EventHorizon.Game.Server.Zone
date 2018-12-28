using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemModelExtensions
    {
        public static void AddSystemModel(this IServiceCollection services)
        {
        }
        public static void UseSystemModel(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
            }
        }
    }
}