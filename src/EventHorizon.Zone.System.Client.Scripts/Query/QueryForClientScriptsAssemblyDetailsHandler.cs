namespace EventHorizon.Zone.System.Client.Scripts.Query
{
    using System;

    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Events.Query;
    using EventHorizon.Zone.System.Client.Scripts.Model.Query;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class QueryForClientScriptsAssemblyDetailsHandler
        : IRequestHandler<QueryForClientScriptsAssemblyDetails, ClientScriptsAssemblyDetails>
    {
        private readonly ClientScriptsState _state;

        public QueryForClientScriptsAssemblyDetailsHandler(
            ClientScriptsState state
        )
        {
            _state = state;
        }

        public Task<ClientScriptsAssemblyDetails> Handle(
            QueryForClientScriptsAssemblyDetails request,
            CancellationToken cancellationToken
        )
        {
            return new ClientScriptsAssemblyDetails(
                _state.Hash
            ).FromResult();
        }
    }
}
