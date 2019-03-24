using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Client.Scripts.Model;
using EventHorizon.Zone.System.Client.Scripts.State;
using MediatR;

namespace EventHorizon.Zone.System.Client.Scripts.Fetch
{
    public class FetchClientScriptListQueryHandler : IRequestHandler<FetchClientScriptListQuery, IEnumerable<ClientScript>>
    {
        readonly ClientScriptRepository _clientScriptRepository;
        public FetchClientScriptListQueryHandler(
            ClientScriptRepository clientScriptRepository
        )
        {
            _clientScriptRepository = clientScriptRepository;
        }
        public Task<IEnumerable<ClientScript>> Handle(
            FetchClientScriptListQuery request, 
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _clientScriptRepository.All()
            );
        }
    }
}