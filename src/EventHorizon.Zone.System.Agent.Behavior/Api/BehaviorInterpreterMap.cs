using EventHorizon.Zone.System.Agent.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Behavior.Api
{
    public interface BehaviorInterpreterMap
    {
        BehaviorInterpreter InterperterByType(
           BehaviorNodeType type
       );
    }
}