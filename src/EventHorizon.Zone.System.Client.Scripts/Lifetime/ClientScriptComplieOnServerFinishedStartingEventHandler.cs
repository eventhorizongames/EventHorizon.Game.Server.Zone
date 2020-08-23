namespace EventHorizon.Zone.System.Client.Scripts.Lifetime
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using MediatR;
    using EventHorizon.Zone.System.Client.Scripts.Compile;

    public class ClientScriptComplieOnServerFinishedStartingEventHandler
        : INotificationHandler<ServerFinishedStartingEvent>
    {
        private readonly IMediator _mediator;

        public ClientScriptComplieOnServerFinishedStartingEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            ServerFinishedStartingEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new CompileClientScriptCommand(),
                cancellationToken
            );
        }
    }
}
