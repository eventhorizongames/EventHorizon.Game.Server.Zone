using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Map
{
    public struct GetMapGraphCommand : IRequest<IMapGraph>
    {
        
    }
}