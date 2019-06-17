using EventHorizon.Zone.System.Agent.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Behavior.Script
{
    public struct BehaviorScriptResponse
    {
        public BehaviorNodeStatus Status { get; }
        public BehaviorScriptResponse(
            BehaviorNodeStatus status
        )
        {
            Status = status;
        }
    }
}