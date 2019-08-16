
using EventHorizon.Zone.System.ClientAssets.Load;
using EventHorizon.Zone.System.ClientAssets.State;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemClientAssetsExtensions
    {
        public static IServiceCollection AddSystemClientAssets(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<ClientAssetRepository, ClientAssetInMemoryRepository>()
            ;
        }
        public static void UseSystemClientAssets(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(new LoadSystemClientAssetsCommand()).GetAwaiter()
                    .GetResult();
            }
        }
    }
}