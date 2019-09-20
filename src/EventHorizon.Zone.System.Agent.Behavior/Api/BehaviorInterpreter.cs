using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Behavior.Api
{
    public interface BehaviorInterpreter
    {
        Task<BehaviorTreeState> Run(
            IObjectEntity actor,
            BehaviorTreeState behaviorTreeState
        );
    }
}