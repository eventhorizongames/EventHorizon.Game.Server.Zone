using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Schedule;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Server.Core.Ping.Tasks
{
    /// <summary>
    /// TODO: Update this to a ITimerTask
    /// PingCoreServerTimerTask : ITimerTask
    /// {
    ///     public int Period { get; } = 1000 * 30; \\ Every 30 Seconds
    ///     public string Tag { get; } = "RunServerActions";
    ///     public INotification OnRunEvent { get; } = new PingCoreServerEvent();
    /// }
    /// </summary>
    public class PingCoreServerScheduledTask : IScheduledTask
    {
        public string Schedule => "*/30 * * * * *";
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PingCoreServerScheduledTask(
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(
            CancellationToken cancellationToken
        )
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                await serviceScope.ServiceProvider.GetService<IMediator>().Publish(new PingCoreServerEvent());
            }
        }
    }
}