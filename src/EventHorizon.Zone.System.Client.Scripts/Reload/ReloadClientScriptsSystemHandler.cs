namespace EventHorizon.Zone.System.Client.Scripts.Reload
{
    using EventHorizon.Zone.System.Client.Scripts.Actions.Reload;
    using EventHorizon.Zone.System.Client.Scripts.Fetch;
    using EventHorizon.Zone.System.Client.Scripts.Load;
    using EventHorizon.Zone.System.Client.Scripts.Model.Client;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class ReloadClientScriptsSystemHandler : IRequestHandler<ReloadClientScriptsSystem>
    {
        private readonly IMediator _mediator;

        public ReloadClientScriptsSystemHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(
            ReloadClientScriptsSystem request, 
            CancellationToken cancellationToken
        )
        {
            // Load Client Scripts
            await _mediator.Publish(
                new LoadClientScriptsSystemCommand()
            );

            // Publish Client Event that Scripts Have Changed
            await _mediator.Publish(
                ClientScriptsSystemReloadedClientActionToAllEvent.Create(
                    new ClientScriptsSystemReloadedClientActionData(
                        await _mediator.Send(
                            new FetchClientScriptListQuery()
                        )
                    )
                )
            );

            return Unit.Value;
        }
    }
}