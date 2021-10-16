namespace EventHorizon.Zone.System.Server.Scripts.Query
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Model.Query;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class QueryForServerScriptsErrorDetailsHandler
        : IRequestHandler<QueryForServerScriptsErrorDetails, CommandResult<ServerScriptsErrorDetailsResponse>>
    {
        private readonly ServerScriptsState _state;

        public QueryForServerScriptsErrorDetailsHandler(
            ServerScriptsState state
        )
        {
            _state = state;
        }

        public Task<CommandResult<ServerScriptsErrorDetailsResponse>> Handle(
            QueryForServerScriptsErrorDetails request,
            CancellationToken cancellationToken
        ) => new CommandResult<ServerScriptsErrorDetailsResponse>(
            new ServerScriptsErrorDetailsResponse(
                !string.IsNullOrEmpty(
                    _state.ErrorCode
                ),
                _state.ErrorCode,
                _state.ErrorDetailsList
            )
        ).FromResult();
    }
}
