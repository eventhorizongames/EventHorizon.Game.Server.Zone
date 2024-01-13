namespace EventHorizon.Zone.Core.Set;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Entity;

using MediatR;

public struct SetEntityPropertyOverrideDataCommand
    : IRequest<CommandResult<IObjectEntity>>
{
    public IObjectEntity Entity { get; }

    public SetEntityPropertyOverrideDataCommand(
        IObjectEntity entity
    )
    {
        Entity = entity;
    }
}
