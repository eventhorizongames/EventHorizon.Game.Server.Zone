using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Register;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace EventHorizon.Game.Server.Zone.Core.Lifetime
{
    public class ShutdownZoneServerHandler : INotificationHandler<ShutdownZoneServerEvent>
    {
        private IMediator _mediator;
        public ShutdownZoneServerHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(ShutdownZoneServerEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new UnregisterWithCoreServerEvent());
        }
    }
}