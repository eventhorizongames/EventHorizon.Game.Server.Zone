namespace EventHorizon.Zone.System.Player.Editor;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

public record QueryForEditorPlayerData() : IRequest<CommandResult<ObjectEntityData>>;
