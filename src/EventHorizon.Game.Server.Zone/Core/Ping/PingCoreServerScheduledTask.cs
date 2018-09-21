using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Schedule;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core.Ping
{
    public class PingCoreServerScheduledTask : IScheduledTask
    {
        public string Schedule => "*/30 * * * * *";
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PingCoreServerScheduledTask(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                await serviceScope.ServiceProvider.GetService<IMediator>().Publish(new PingCoreServerEvent());
            }
        }
    }
}