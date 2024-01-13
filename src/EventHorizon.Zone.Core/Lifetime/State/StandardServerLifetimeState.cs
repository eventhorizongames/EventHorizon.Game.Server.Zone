namespace EventHorizon.Zone.Core.Lifetime.State;

public class StandardServerLifetimeState : ServerLifetimeState
{
    bool _isServerStarted = false;

    public bool IsServerStarted()
    {
        return _isServerStarted;
    }

    public bool SetServerStarted(
        bool isServerStarted
    )
    {
        _isServerStarted = isServerStarted;
        return _isServerStarted;
    }
}
