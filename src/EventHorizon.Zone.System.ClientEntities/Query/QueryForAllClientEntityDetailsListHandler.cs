namespace EventHorizon.Zone.System.ClientEntities.Query
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.ClientEntities.State;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class QueryForAllClientEntityDetailsListHandler : IRequestHandler<QueryForAllClientEntityDetailsList, IEnumerable<IObjectEntity>>
    {
        private readonly ClientEntityRepository _clientEntityRepository;

        public QueryForAllClientEntityDetailsListHandler(
            ClientEntityRepository clientEntityRepository
        )
        {
            _clientEntityRepository = clientEntityRepository;
        }

        public Task<IEnumerable<IObjectEntity>> Handle(
            QueryForAllClientEntityDetailsList request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _clientEntityRepository.All()
                    .Cast<IObjectEntity>()
            );
        }
    }
}