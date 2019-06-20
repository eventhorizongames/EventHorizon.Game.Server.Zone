using EventHorizon.TimerService;
using EventHorizon.Zone.System.Agent.Behavior.Update;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Behavior.Timer
{
    public class BehaviorTreeUpdateTriggerTimerTask : ITimerTask
    {
        public int Period { get; } = 100;
        public string Tag { get; } = "RunUpdateOnAllBehaviorTrees";
        public INotification OnRunEvent { get; } = new RunUpdateOnAllBehaviorTrees();
    }
}