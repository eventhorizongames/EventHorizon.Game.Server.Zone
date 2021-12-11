namespace EventHorizon.Zone.System.ArtifactManagement.Tasks;

using EventHorizon.BackgroundTasks.Model;

using global::System;

public class ImportZoneDataTask
    : BackgroundTask
{
    public string ReferenceId { get; } = Guid.NewGuid().ToString();
    public string ImportArtifactUrl { get; }

    public ImportZoneDataTask(
        string importArtifactUrl
    )
    {
        ImportArtifactUrl = importArtifactUrl;
    }
}
