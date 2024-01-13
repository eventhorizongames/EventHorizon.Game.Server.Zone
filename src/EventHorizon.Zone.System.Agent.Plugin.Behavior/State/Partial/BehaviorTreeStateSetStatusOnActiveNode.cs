namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

public partial struct BehaviorTreeState
{
    public BehaviorTreeState SetStatusOnActiveNode(
        BehaviorNodeStatus status
    )
    { // TODO: Create Unit Test
        if (status.Equals(default(BehaviorNodeStatus)))
        {
            status = BehaviorNodeStatus.ERROR;
        }
        NodeMap[_activeNodeToken] = ActiveNode.UpdateStatus(
            status.ToString()
        );
        return this;
    }
}
