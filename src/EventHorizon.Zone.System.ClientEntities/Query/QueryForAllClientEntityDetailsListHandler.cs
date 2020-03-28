namespace EventHorizon.Zone.System.ClientEntities.Query
{
    using EventHorizon.Zone.System.Agent.Save.Mapper;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.State;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class QueryForAllClientEntityDetailsListHandler : IRequestHandler<QueryForAllClientEntityDetailsList, IEnumerable<ClientEntityDetails>>
    {
        private readonly ClientEntityRepository _clientEntityRepository;

        public QueryForAllClientEntityDetailsListHandler(
            ClientEntityRepository clientEntityRepository
        )
        {
            _clientEntityRepository = clientEntityRepository;
        }

        public Task<IEnumerable<ClientEntityDetails>> Handle(
            QueryForAllClientEntityDetailsList request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                MapEntityListToDetails(
                    _clientEntityRepository.All()
                )
            );
        }

        private IEnumerable<ClientEntityDetails> MapEntityListToDetails(
            IEnumerable<ClientEntity> clientList
        )
        {
            var result = new List<ClientEntityDetails>();
            foreach (var entity in clientList)
            {
                result.Add(
                    ClientEntityFromEntityToDetails.Map(
                        entity
                    )
                );
            }
            return result;
        }
    }
}