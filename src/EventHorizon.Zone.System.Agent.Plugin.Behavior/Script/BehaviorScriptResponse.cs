using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Script
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