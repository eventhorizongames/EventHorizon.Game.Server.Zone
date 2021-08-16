namespace EventHorizon.Server.Core.State
{
    public class SystemServerCoreCheckState : ServerCoreCheckState
    {
        private int timesChecked = 0;

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
