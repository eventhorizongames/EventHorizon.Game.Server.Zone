using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Agent.Save.Events;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Save
{
    public class SaveAgentStateTimerTask : ITimerTask
    {
        public int Period { get; } = 5000;
        public string Tag { get; } = "SaveAgentStateEvent";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new SaveAgentStateEvent();
    }
}