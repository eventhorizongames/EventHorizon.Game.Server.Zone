namespace EventHorizon.Zone.System.Player.Set;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Player;

using MediatR;

public struct SetPlayerPropertyOverrideDataCommand
    : IRequest<CommandResult<PlayerEntity>>
{
    public PlayerEntity PlayerEntity { get; }

    public SetPlayerPropertyOverrideDataCommand(
        PlayerEntity playerEntity
    )
    {
        PlayerEntity = playerEntity;
    }
}
