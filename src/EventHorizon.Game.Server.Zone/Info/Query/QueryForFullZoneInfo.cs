using EventHorizon.Game.Server.Zone.Info.Api;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Info.Query
{
    public struct QueryForFullZoneInfo : IRequest<IZoneInfo>
    {
        
    }
}