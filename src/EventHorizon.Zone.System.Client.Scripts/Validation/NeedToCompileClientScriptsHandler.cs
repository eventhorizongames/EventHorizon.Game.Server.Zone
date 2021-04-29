namespace EventHorizon.Zone.System.Client.Scripts.Validation
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Consolidate;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Create;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class NeedToCompileClientScriptsHandler
        : IRequestHandler<NeedToCompileClientScripts, CommandResult<bool>>
    {
        private readonly IMediator _mediator;
        private readonly ClientScriptRepository _clientScriptRepository;
        private readonly ClientScriptsState _state;

        public NeedToCompileClientScriptsHandler(
            IMediator mediator,
            ClientScriptRepository clientScriptRepository,
            ClientScriptsState state
        )
        {
            _mediator = mediator;
            _clientScriptRepository = clientScriptRepository;
            _state = state;
        }

        public async Task<CommandResult<bool>> Handle(
            NeedToCompileClientScripts request,
            CancellationToken cancellationToken
        )
        {
            // Consolidate Scripts
            var consolidatedResult = await _mediator.Send(
                new ConsolidateClientScriptsCommand(
                    _clientScriptRepository.All().Where(
                        a => a.ScriptType == Model.ClientScriptType.CSharp
                     )
                ),
                cancellationToken
            );
            if (!consolidatedResult.Success)
            {
                return new(
                    consolidatedResult.ErrorCode
                );
            }

            // Get Current Hash
            var hashResult = await _mediator.Send(
                new CreateHashFromContentCommand(
                    consolidatedResult.Result.ConsolidatedScripts
                ),
                cancellationToken
            );
            if (!hashResult.Success)
            {
                return new(
                    hashResult.ErrorCode
                );
            }

            if (_state.Hash != hashResult.Result)
            {
                return new(
                    true
                );
            }

            return new(
                false
            );
        }
    }
}
