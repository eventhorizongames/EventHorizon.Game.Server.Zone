namespace EventHorizon.Zone.Core.Entity.Reload
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Entity.Load;
    using EventHorizon.Zone.Core.Events.Entity.Reload;
    using EventHorizon.Zone.Core.Model.Command;

    using MediatR;

    public class ReloadEntityCoreCommandHandler
        : IRequestHandler<ReloadEntityCoreCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;

        public ReloadEntityCoreCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<StandardCommandResult> Handle(
            ReloadEntityCoreCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(
                new LoadEntityCoreCommand(),
                cancellationToken
            );

            if (result
                && result.WasUpdated
            )
            {
                // TODO: Add ClientAction to reload Entity Core on Clients
            }

            return new();
        }
    }
}
