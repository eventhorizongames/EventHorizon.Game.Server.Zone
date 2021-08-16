using System;
using System.Collections.Generic;

using EventHorizon.Zone.System.Server.Scripts.Model.Details;

using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.Events.Query
{
    public struct QueryForServerScriptDetails : IRequest<IEnumerable<ServerScriptDetails>>
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
