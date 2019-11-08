using MediatR;

namespace EventHorizon.Zone.Core.Events.Lifetime
{
    public struct FinishServerStartCommand : IRequest<bool>
    {

    }
}