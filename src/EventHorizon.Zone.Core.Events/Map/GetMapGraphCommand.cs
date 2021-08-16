namespace EventHorizon.Zone.Core.Events.Map
{
    using EventHorizon.Zone.Core.Model.Map;

    using MediatR;

    public struct GetMapGraphCommand : IRequest<IMapGraph>
    {

    }
}
