namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Api
{
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

    public interface BehaviorInterpreter
    {
        Task<BehaviorTreeState> Run(
            IObjectEntity actor,
            BehaviorTreeState behaviorTreeState
        );
    }
}
