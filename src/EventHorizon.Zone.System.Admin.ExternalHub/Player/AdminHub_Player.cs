namespace EventHorizon.Zone.System.Admin.ExternalHub;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Player.Editor;
using EventHorizon.Zone.System.Player.Model.Settings;
using global::System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Make sure this stays on the ExternalHub root namespace.
/// This Class is encapsulating the Zone Info related logic,
///  and allows for a single SignalR hub to host all APIs.
/// </summary>
public partial class AdminHub : Hub
{
    public Task<CommandResult<ObjectEntityData>> Player_GetData() =>
        _mediator.Send(new QueryForEditorPlayerData(), Context.ConnectionAborted);

    public Task<StandardCommandResult> Player_SaveData(PlayerObjectEntityDataModel data) =>
        _mediator.Send(new SaveEditorPlayerData(data), Context.ConnectionAborted);
}
