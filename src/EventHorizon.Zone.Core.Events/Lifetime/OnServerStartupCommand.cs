namespace EventHorizon.Zone.Core.Events.Lifetime;

using EventHorizon.Zone.Core.Model.Lifetime;

using MediatR;

public interface OnServerStartupCommand
    : IRequest<OnServerStartupResult>
{
}
