namespace EventHorizon.Zone.System.Server.Scripts.Set
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using MediatR;
    using EventHorizon.Zone.Core.Model.Command;

    public class SetServerScriptDetailsCommandHandler
        : IRequestHandler<SetServerScriptDetailsCommand, StandardCommandResult>
    {
        private readonly ServerScriptDetailsRepository _serverScriptDetailsRepository;

        public SetServerScriptDetailsCommandHandler(
            ServerScriptDetailsRepository serverScriptDetailsRepository
        )
        {
            _serverScriptDetailsRepository = serverScriptDetailsRepository;
        }

        public Task<StandardCommandResult> Handle(
            SetServerScriptDetailsCommand request,
            CancellationToken cancellationToken
        )
        {
            _serverScriptDetailsRepository.Add(
                request.ScriptDetails.Id,
                request.ScriptDetails
            );
            return new StandardCommandResult()
                .FromResult();
        }
    }
}
