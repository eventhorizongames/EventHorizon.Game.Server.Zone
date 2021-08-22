namespace EventHorizon.Game.Server.Zone.Info.Query
{
    using System.Collections.Generic;

    using MediatR;

    public struct QueryForFullZoneInfo : IRequest<IDictionary<string, object>>
    {

    }
}
