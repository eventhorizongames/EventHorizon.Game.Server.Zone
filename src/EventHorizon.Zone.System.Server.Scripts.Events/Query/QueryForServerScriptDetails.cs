namespace EventHorizon.Zone.System.Server.Scripts.Events.Query
{
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;

    using global::System;
    using global::System.Collections.Generic;

    using MediatR;

    public struct QueryForServerScriptDetails
        : IRequest<IEnumerable<ServerScriptDetails>>
    {
        public Func<ServerScriptDetails, bool> Query { get; }

        public QueryForServerScriptDetails(
            Func<ServerScriptDetails, bool> query
        )
        {
            Query = query;
        }
    }
}
