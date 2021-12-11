namespace EventHorizon.Zone.System.ArtifactManagement.Tasks;

using EventHorizon.BackgroundTasks.Model;
using EventHorizon.Zone.System.ArtifactManagement.ClientActions;
using EventHorizon.Zone.System.ArtifactManagement.Import;

using global::System;

public record RevertToBackupZoneDataTask
    : BackgroundTask
{
    public string ReferenceId { get; init; } = Guid.NewGuid().ToString();
    public string BackupArtifactUrl { get; }

    public RevertToBackupZoneDataTask(
        string backupArtifactUrl
    )
    {
        BackupArtifactUrl = backupArtifactUrl;
    }
}
