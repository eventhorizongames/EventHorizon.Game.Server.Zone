namespace EventHorizon.Game.Server.Zone
{

    using EventHorizon.Zone.System.Particle.Load;
    using EventHorizon.Zone.System.Particle.State;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

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
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(
                    new LoadParticleSystemEvent()
                ).GetAwaiter().GetResult();
            }

            return app;
        }
    }
}