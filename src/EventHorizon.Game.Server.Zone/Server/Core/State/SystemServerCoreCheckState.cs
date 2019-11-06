namespace EventHorizon.Game.Server.Zone.Server.Core.State
{
    public class SystemServerCoreCheckState : ServerCoreCheckState
    {
        int timesChecked = 0;
        
        public void Check()
        {
            timesChecked += 1;
        }

        public void Reset()
        {
            timesChecked = 0;
        }

        public int TimesChecked()
        {
            return timesChecked;
        }
    }
}