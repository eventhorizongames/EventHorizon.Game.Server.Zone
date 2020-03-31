namespace EventHorizon.Zone.System.ClientEntities.Query
{
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Agent.Save.Mapper;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.State;
    using MediatR;

    public class QueryForAllRawClientEntityDetailsListHandler : IRequestHandler<QueryForAllRawClientEntityDetailsList, IEnumerable<ClientEntityDetails>>
    {
        private readonly ClientEntityRepository _clientEntityRepository;

        public QueryForAllRawClientEntityDetailsListHandler(
            ClientEntityRepository clientEntityRepository
        )
        {
            _clientEntityRepository = clientEntityRepository;
        }

        public Task<IEnumerable<ClientEntityDetails>> Handle(
            QueryForAllRawClientEntityDetailsList request,
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
                var details = ClientEntityFromEntityToDetails.Map(
                    entity
                );
                foreach (var prop in entity.RawData)
                {
                    if (!details.Data.ContainsKey(prop.Key))
                    {
                        details.Data.Add(
                            prop.Key,
                            prop.Value
                        );
                    }
                }

                result.Add(
                    details
                );
            }
            return result;
        }
    }
}