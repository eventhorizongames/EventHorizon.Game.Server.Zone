namespace EventHorizon.Zone.System.ClientEntities.Query
{
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.State;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class QueryForAllRawClientEntityDetailsListHandler : IRequestHandler<QueryForAllRawClientEntityDetailsList, IEnumerable<ClientEntity>>
    {
        private readonly ClientEntityRepository _clientEntityRepository;

        public QueryForAllRawClientEntityDetailsListHandler(
            ClientEntityRepository clientEntityRepository
        )
        {
            _clientEntityRepository = clientEntityRepository;
        }

        public Task<IEnumerable<ClientEntity>> Handle(
            QueryForAllRawClientEntityDetailsList request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _clientEntityRepository.All()
            );
        }
    }
}
