namespace EventHorizon.Zone.System.Client.Scripts.Events.Fetch
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using MediatR;

    public struct FetchClientScriptListQuery 
        : IRequest<IEnumerable<ClientScript>>
    {

    }
}