using EventHorizon.Zone.Core.Reporter.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {   
        public BehaviorTreeState Report(
            string message,
            object data = null
        )
        {
            _reportTracker?.Track(
                _reportId,
                message,
                data ?? ""
            );
            return this;
        }
        
        public BehaviorTreeState ClearReport() 
        {
            _reportTracker?.Clear(
                _reportId
            );
            return this;
        }

        public BehaviorTreeState SetReportTracker(
            string reportId,
            ReportTracker reportTracker
        )
        {
            _reportId = reportId;
            _reportTracker = reportTracker;
            return this;
        }
    }
}