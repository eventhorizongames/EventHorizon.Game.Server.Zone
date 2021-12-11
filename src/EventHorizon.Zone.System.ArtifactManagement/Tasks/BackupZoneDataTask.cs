namespace EventHorizon.Zone.System.ArtifactManagement.Tasks;

using EventHorizon.BackgroundTasks.Model;

using global::System;

public record BackupZoneDataTask
    : BackgroundTask
{
    public string ReferenceId { get; init; } = Guid.NewGuid().ToString();
}
