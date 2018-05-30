using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace EventHorizon.Game.Server.Zone.Core.Lifetime
{
    public class RegisterZoneServerShutdownHandler : INotificationHandler<RegisterZoneServerShutdownEvent>
    {
        private IMediator _mediator;
        private IApplicationLifetime _applicationLifetime;
        public RegisterZoneServerShutdownHandler(IMediator mediator, IApplicationLifetime applicationLifetime)
        {
            _mediator = mediator;
            _applicationLifetime = applicationLifetime;
        }

        public Task Handle(RegisterZoneServerShutdownEvent notification, CancellationToken cancellationToken)
        {
            _applicationLifetime.ApplicationStopping.Register(() => this.OnApplicationStopping());
            return Task.CompletedTask;
        }

        public void OnApplicationStopping()
        {
            _mediator.Publish(new ShutdownZoneServerEvent()).GetAwaiter();
        }
    }
}