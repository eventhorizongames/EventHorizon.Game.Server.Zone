namespace EventHorizon.Zone.System.Client.Scripts.Fetch
{
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Events.Fetch;
    using EventHorizon.Zone.System.Client.Scripts.Model;

    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class FetchClientScriptListQueryHandler
        : IRequestHandler<FetchClientScriptListQuery, IEnumerable<ClientScript>>
    {
        private readonly ClientScriptRepository _clientScriptRepository;

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
                _clientScriptRepository.All().Where(
                    a => a.ScriptType == ClientScriptType.JavaScript
                )
            );
        }
    }
}
