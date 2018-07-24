using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Core.Connection.Stop.Handler
{
    public class StopZoneCoreConnectionHandler : INotificationHandler<StopZoneCoreConnectionEvent>
    {
        ICoreConnectionCache _connectionCache;
        public StopZoneCoreConnectionHandler(ICoreConnectionCache connectionCache)
        {
            _connectionCache = connectionCache;
        }
        public async Task Handle(StopZoneCoreConnectionEvent notification, CancellationToken cancellationToken)
        {
            await _connectionCache.Stop();
        }
    }
}