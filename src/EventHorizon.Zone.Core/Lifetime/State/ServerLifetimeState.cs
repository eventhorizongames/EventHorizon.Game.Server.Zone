namespace EventHorizon.Zone.Core.Lifetime.State;

public interface ServerLifetimeState
{
    bool IsServerStarted();
    bool SetServerStarted(
        bool isServerStarted
    );
}
