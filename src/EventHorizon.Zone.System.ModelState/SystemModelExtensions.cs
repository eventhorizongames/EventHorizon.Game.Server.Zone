using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Zone.System.ModelState
{
    public static class SystemModelExtensions
    {
        public static IServiceCollection AddSystemModelState(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static void UseSystemModelState(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
            }
        }
    }
}