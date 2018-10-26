using EventHorizon.Game.Server.Zone.Particle.State;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Particle
{
    public static class ParticleExtensions
    {
        public static void AddParticle(this IServiceCollection services)
        {
            services
                .AddSingleton<ParticleState, ParticleTemplateContainer>();
        }
        public static void UseParticle(this IApplicationBuilder app)
        {
            // using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            // {
            // }
        }
    }
}