namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Api
{
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

    public interface BehaviorInterpreterMap
    {
        BehaviorInterpreter InterperterByType(
           BehaviorNodeType type
       );
    }
}
