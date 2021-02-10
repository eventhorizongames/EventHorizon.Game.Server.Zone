namespace EventHorizon.Zone.System.Client.Server.Lifetime
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using MediatR;
    using EventHorizon.Zone.System.Server.Scripts.Complie;
    using EventHorizon.Zone.System.Server.Scripts.Load;

    public class ServerScriptsComplieOnServerFinishedStartingEventHandler
        : INotificationHandler<ServerFinishedStartingEvent>
    {
        private readonly IMediator _mediator;

        public ServerScriptsComplieOnServerFinishedStartingEventHandler(
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
                new LoadServerScriptsCommand(),
                cancellationToken
            );

            await _mediator.Send(
                new CompileServerScriptsFromSubProcessCommand(),
                cancellationToken
            );
        }
    }
}
