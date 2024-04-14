namespace EventHorizon.Zone.System.Player.Editor;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Player.Model.Settings;
using MediatR;

public record SaveEditorPlayerData(PlayerObjectEntityDataModel PlayerData)
    : IRequest<StandardCommandResult>;
