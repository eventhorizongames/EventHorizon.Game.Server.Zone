using MediatR;

namespace EventHorizon.Zone.Core.Events.Lifetime
{
    public struct IsServerStarted : IRequest<bool>
    {

    }
}