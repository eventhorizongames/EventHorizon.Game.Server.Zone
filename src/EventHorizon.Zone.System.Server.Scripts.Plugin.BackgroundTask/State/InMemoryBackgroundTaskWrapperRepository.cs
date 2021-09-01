namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.State
{
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;

    using global::System.Collections.Concurrent;
    using global::System.Diagnostics.CodeAnalysis;

    public class InMemoryBackgroundTaskWrapperRepository
        : BackgroundTaskWrapperRepository
    {
        private readonly ConcurrentDictionary<string, BackgroundTaskWrapper> _map = new();

        public bool Add(
            string taskId,
            BackgroundTaskWrapper backgroundTaskWrapper
        )
        {
            return _map.TryAdd(
                taskId,
                backgroundTaskWrapper
            );
        }

        public bool TryGet(
            string taskId,
            [NotNullWhen(true)]
            out BackgroundTaskWrapper? backgroundTaskWrapper
        )
        {
            return _map.TryGetValue(
                taskId,
                out backgroundTaskWrapper
            );
        }

        public bool TryRemove(
            string taskId,
            [NotNullWhen(true)]
            out BackgroundTaskWrapper? removedBackgroundTaskWrapper
        )
        {
            return _map.TryRemove(
                taskId,
                out removedBackgroundTaskWrapper
            );
        }
    }
}
