namespace EventHorizon.TimerService
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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