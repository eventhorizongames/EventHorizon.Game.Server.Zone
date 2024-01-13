namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;

public interface BackgroundTaskWrapper
{
    void Start();

    void Resume();

    void Stop();

    void Dispose();
}
