namespace EventHorizon.Zone.System.Admin.ExternalHub;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Server.Scripts.Model.Query;

using global::System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Make sure this stays on the ExternalHub root namespace.
/// This Class is encapsulating the Command related logic,
///  and allows for a single SignalR hub to host all APIs.
/// </summary>
public partial class AdminHub
    : Hub
{
    public Task<CommandResult<ServerScriptsErrorDetailsResponse>> ServerScripts_ErrorDetails()
        => _mediator.Send(
            new QueryForServerScriptsErrorDetails(),
            Context.ConnectionAborted
        );
}
