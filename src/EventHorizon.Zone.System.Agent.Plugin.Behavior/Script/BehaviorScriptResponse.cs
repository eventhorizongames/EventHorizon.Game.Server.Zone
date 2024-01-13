namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;

using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Server.Scripts.Model;

// TODO: Move this to under the Model Namespace
public struct BehaviorScriptResponse : ServerScriptResponse
{
    public BehaviorNodeStatus Status { get; }

    public bool Success => true;
    public string Message => string.Empty;

    public BehaviorScriptResponse(
        BehaviorNodeStatus status
    )
    {
        Status = status;
    }
}
