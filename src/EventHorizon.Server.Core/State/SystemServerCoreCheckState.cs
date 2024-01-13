namespace EventHorizon.Server.Core.State;

public class SystemServerCoreCheckState
    : ServerCoreCheckState
{
    private int _timesChecked = 0;

    public void Check()
    {
        _timesChecked += 1;
    }

    public void Reset()
    {
        _timesChecked = 0;
    }

    public int TimesChecked()
    {
        return _timesChecked;
    }
}
