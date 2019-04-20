using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.ClientEntities.Api;
using EventHorizon.Zone.System.ClientEntities.State;
using MediatR;

namespace EventHorizon.Zone.System.ClientEntities.Fetch
{
    public struct FetchClientEntityInstanceListQueryHandler : IRequestHandler<FetchClientEntityInstanceListQuery, IEnumerable<IClientEntityInstance>>
    {
        readonly ClientEntityInstanceRepository _entityInstanceRepository;
        public FetchClientEntityInstanceListQueryHandler(
            ClientEntityInstanceRepository entityInstanceRepository
        )
        {
            _entityInstanceRepository = entityInstanceRepository;
        }
        public Task<IEnumerable<IClientEntityInstance>> Handle(
            FetchClientEntityInstanceListQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _entityInstanceRepository.All()
            );
        }
    }
}