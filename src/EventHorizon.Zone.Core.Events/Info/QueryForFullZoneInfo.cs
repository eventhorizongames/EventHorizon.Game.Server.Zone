using System.Collections.Generic;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Info.Query
{
    public struct QueryForFullZoneInfo : IRequest<IDictionary<string, object>>
    {
        
    }
}