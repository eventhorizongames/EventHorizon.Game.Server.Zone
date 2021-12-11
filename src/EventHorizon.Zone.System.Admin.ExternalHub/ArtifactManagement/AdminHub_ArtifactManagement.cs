namespace EventHorizon.Zone.System.Admin.ExternalHub;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ArtifactManagement.Trigger;

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
    public Task<CommandResult<TriggerZoneArtifactBackupResult>> ArtifactManagement_TriggerBackup()
        => _mediator.Send(
            new TriggerZoneArtifactBackupCommand(),
            Context.ConnectionAborted
        );

    public Task<CommandResult<TriggerZoneArtifactExportResult>> ArtifactManagement_TriggerExport()
        => _mediator.Send(
            new TriggerZoneArtifactExportCommand(),
            Context.ConnectionAborted
        );

    public Task<CommandResult<TriggerZoneArtifactImportResult>> ArtifactManagement_TriggerImport(
        string importArtifactUrl
    ) => _mediator.Send(
        new TriggerZoneArtifactImportCommand(
            importArtifactUrl
        ),
        Context.ConnectionAborted
    );
}
