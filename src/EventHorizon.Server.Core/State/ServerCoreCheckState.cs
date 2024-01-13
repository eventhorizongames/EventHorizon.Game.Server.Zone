namespace EventHorizon.Server.Core.State;

public interface ServerCoreCheckState
{
    void Reset();
    int TimesChecked();
    void Check();
}
