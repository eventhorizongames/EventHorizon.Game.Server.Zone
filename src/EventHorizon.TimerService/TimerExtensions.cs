
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventHorizon.TimerService
{
    public static class TimerExtensions
    {
        public static IServiceCollection AddTimer(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<IHostedService, TimerHostedService>()
            ;
        }
    }
}