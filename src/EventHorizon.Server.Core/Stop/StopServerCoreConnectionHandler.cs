namespace EventHorizon.Server.Core.Stop;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Server.Core.Connection;
using EventHorizon.Server.Core.Events.Stop;

using MediatR;

public class StopCoreServerConnectionHandler : INotificationHandler<StopCoreServerConnection>
{
    private readonly CoreServerConnectionCache _connectionCache;

    public StopCoreServerConnectionHandler(
        CoreServerConnectionCache connectionCache
    )
    {
        _connectionCache = connectionCache;
    }

    public async Task Handle(
        StopCoreServerConnection notification,
        CancellationToken cancellationToken
    )
    {
        await _connectionCache.Stop();
    }
}
