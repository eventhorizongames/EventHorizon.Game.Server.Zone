namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;

using global::System.Diagnostics.CodeAnalysis;

public interface BackgroundTaskWrapperRepository
{
    bool TryGet(
        string taskId,
        [NotNullWhen(true)]
        out BackgroundTaskWrapper? backgroundTaskWrapper
    );

    bool Add(
        string taskId,
        BackgroundTaskWrapper backgroundTaskWrapper
    );

    bool TryRemove(
        string taskId,
        [NotNullWhen(true)]
        out BackgroundTaskWrapper? removedBackgroundTaskWrapper
    );
}
