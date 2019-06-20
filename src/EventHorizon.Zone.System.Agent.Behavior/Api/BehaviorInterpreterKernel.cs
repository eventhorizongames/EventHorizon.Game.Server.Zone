using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Behavior.Api
{
    public interface BehaviorInterpreterKernel
    {
        Task<BehaviorTreeState> Tick(
            ActorBehaviorTreeShape shape,
            IObjectEntity actor
        );
    }
}