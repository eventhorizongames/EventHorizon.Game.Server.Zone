namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api
{
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;

    public interface BackgroundTaskWrapperBuilder
{
        BackgroundTaskWrapper Build(
            ScriptedBackgroundTask task
        );
    }
}
