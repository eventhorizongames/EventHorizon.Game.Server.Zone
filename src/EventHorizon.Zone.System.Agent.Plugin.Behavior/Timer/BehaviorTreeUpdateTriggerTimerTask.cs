using EventHorizon.TimerService;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Update;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer
{
    public class BehaviorTreeUpdateTriggerTimerTask : ITimerTask
    {
        public int Period { get; } = 100;
        public string Tag { get; } = "RunUpdateOnAllBehaviorTrees";
        public INotification OnRunEvent { get; } = new RunUpdateOnAllBehaviorTrees();
    }
}