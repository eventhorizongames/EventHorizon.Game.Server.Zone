namespace EventHorizon.Zone.System.Client.Scripts.Query;

using System;

using EventHorizon.Zone.System.Client.Scripts.Api;
using EventHorizon.Zone.System.Client.Scripts.Events.Query;
using EventHorizon.Zone.System.Client.Scripts.Model.Query;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class QueryForClientScriptsAssemblyHandler
    : IRequestHandler<QueryForClientScriptsAssembly, ClientScriptsAssemblyResult>
{
    private readonly ClientScriptsState _state;

    public QueryForClientScriptsAssemblyHandler(
        ClientScriptsState state
    )
    {
        _state = state;
    }

    public Task<ClientScriptsAssemblyResult> Handle(
        QueryForClientScriptsAssembly request,
        CancellationToken cancellationToken
    )
    {
        return new ClientScriptsAssemblyResult(
            _state.Hash,
            _state.ScriptAssembly
        ).FromResult();
    }
}
