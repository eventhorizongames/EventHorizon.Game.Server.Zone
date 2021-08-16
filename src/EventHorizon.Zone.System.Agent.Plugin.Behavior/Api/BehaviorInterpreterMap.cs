using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Api
{
    public interface BehaviorInterpreterMap
    {
        BehaviorInterpreter InterperterByType(
           BehaviorNodeType type
       );
    }
}
