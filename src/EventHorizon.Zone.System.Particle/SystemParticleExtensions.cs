
using EventHorizon.Zone.System.Particle.State;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemParticleExtensions
    {
        public static IServiceCollection AddSystemParticle(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<ParticleTemplateRepository, StandardParticleTemplateRepository>()
            ;
        }
        public static IApplicationBuilder UseSystemParticle(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
            }
            return app;
        }
    }
}