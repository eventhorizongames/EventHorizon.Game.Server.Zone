namespace EventHorizon.Zone.System.Admin.ExternalHub;

using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Builder;

using global::System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Make sure this stays on the ExternalHub root namespace.
/// This Class is encapsulating the Command related logic,
///  and allows for a single SignalR hub to host all APIs.
/// </summary>
public partial class AdminHub : Hub
{
    public Task Command(
        string command,
        object data
    ) => _mediator.Publish(
        new AdminCommandEvent(
            Context.ConnectionId,
            BuildAdminCommand.FromString(
                command
            ),
            data
        ),
        Context.ConnectionAborted
    );
}
