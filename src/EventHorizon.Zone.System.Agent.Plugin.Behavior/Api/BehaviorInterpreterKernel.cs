using System.Threading.Tasks;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Api
{
    public interface BehaviorInterpreterKernel
    {
        Task<BehaviorTreeState> Tick(
            ActorBehaviorTreeShape shape,
            IObjectEntity actor
        );
    }
}
