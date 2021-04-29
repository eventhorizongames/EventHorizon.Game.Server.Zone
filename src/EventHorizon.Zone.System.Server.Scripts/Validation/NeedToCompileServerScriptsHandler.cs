namespace EventHorizon.Zone.System.Server.Scripts.Validation
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Consolidate;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Create;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class NeedToCompileServerScriptsHandler
        : IRequestHandler<NeedToCompileServerScripts, CommandResult<bool>>
    {
        private readonly IMediator _mediator;
        private readonly ServerScriptsState _state;
        private readonly ServerScriptDetailsRepository _serverScriptDetailsRepository;

        public NeedToCompileServerScriptsHandler(
            IMediator mediator,
            ServerScriptsState state,
            ServerScriptDetailsRepository serverScriptDetailsRepository
        )
        {
            _mediator = mediator;
            _state = state;
            _serverScriptDetailsRepository = serverScriptDetailsRepository;
        }

        public async Task<CommandResult<bool>> Handle(
            NeedToCompileServerScripts request,
            CancellationToken cancellationToken
        )
        {
            // Consolidate Scripts
            var consolidatedResult = await _mediator.Send(
                new ConsolidateServerScriptsCommand(
                    _serverScriptDetailsRepository.All
                ),
                cancellationToken
            );
            if (!consolidatedResult.Success)
            {
                return new CommandResult<bool>(
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
                return new CommandResult<bool>(
                    hashResult.ErrorCode
                );
            }

            if (_state.CurrentHash != hashResult.Result)
            {
                return new CommandResult<bool>(
                    true
                );
            }

            return new CommandResult<bool>(
                false
            );
        }
    }
}
