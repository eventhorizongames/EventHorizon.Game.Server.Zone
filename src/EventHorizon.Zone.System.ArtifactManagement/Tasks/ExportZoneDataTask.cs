namespace EventHorizon.Zone.System.ArtifactManagement.Tasks;

using EventHorizon.BackgroundTasks.Model;

using global::System;

public class ExportZoneDataTask
    : BackgroundTask
{
    public string ReferenceId { get; } = Guid.NewGuid().ToString();
}
