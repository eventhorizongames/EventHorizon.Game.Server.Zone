namespace EventHorizon.Zone.System.Server.Scripts.Query
{
    using EventHorizon.Zone.System.Server.Scripts.Events.Query;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.State;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class QueryForServerScriptDetailsHandler
        : IRequestHandler<QueryForServerScriptDetails, IEnumerable<ServerScriptDetails>>
    {
        private readonly ServerScriptDetailsRepository _repository;

        public QueryForServerScriptDetailsHandler(
            ServerScriptDetailsRepository repository
        )
        {
            _repository = repository;
        }

        public Task<IEnumerable<ServerScriptDetails>> Handle(
            QueryForServerScriptDetails request,
            CancellationToken cancellationToken
        )
        {
            return _repository.Where(
                request.Query
            ).FromResult();
        }
    }
}
