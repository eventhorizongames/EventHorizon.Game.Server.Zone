namespace EventHorizon.Zone.Core.Events.Entity.Reload;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public struct ReloadEntityCoreCommand
    : IRequest<StandardCommandResult>
{
}
