using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Events.Query;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;
using EventHorizon.Zone.System.Server.Scripts.State;
using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.Query
{
    public class QueryForServerScriptDetailsHandler : IRequestHandler<QueryForServerScriptDetails, IEnumerable<ServerScriptDetails>>
    {
        readonly ServerScriptDetailsRepository _repository;

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
            return Task.FromResult(
                _repository.Where(
                    request.Query
                )
            );
        }
    }
}