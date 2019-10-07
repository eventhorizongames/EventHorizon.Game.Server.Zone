using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Server.Core.External.Connection;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Server.Core.Stop
{
    public class StopCoreServerConnectionEventHandler : INotificationHandler<StopCoreServerConnectionEvent>
    {
        CoreServerConnectionCache _connectionCache;

        public StopCoreServerConnectionEventHandler(
            CoreServerConnectionCache connectionCache
        )
        {
            _connectionCache = connectionCache;
        }
        
        public async Task Handle(
            StopCoreServerConnectionEvent notification, 
            CancellationToken cancellationToken
        )
        {
            await _connectionCache.Stop();
        }
    }
}