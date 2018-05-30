using EventHorizon.Game.Server.Zone.Core.Lifetime;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.Register;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class CoreExtensions
    {
        public static void AddZoneCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthSettings>(configuration.GetSection("Auth"));
            services.Configure<CoreSettings>(configuration.GetSection("Core"));
            services.Configure<ZoneSettings>(configuration.GetSection("Zone"));
        }
        public static void UseZoneCore(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new FillServerPropertiesEvent()).GetAwaiter().GetResult();
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new RegisterWithCoreServerEvent()).GetAwaiter().GetResult();
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new RegisterZoneServerShutdownEvent()).GetAwaiter().GetResult();
            }
        }
    }
}