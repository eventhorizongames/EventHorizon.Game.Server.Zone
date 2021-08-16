namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreter
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

    using global::System.Threading.Tasks;

    public class BehaviorInterpreterDoWhileKernel : BehaviorInterpreterKernel
    {
        readonly BehaviorInterpreterMap _interpreterMap;
        readonly ReportTracker _reportTracker;

        public BehaviorInterpreterDoWhileKernel(
            BehaviorInterpreterMap interpreterMap,
            ReportTracker reportTracker
        )
        {
            _interpreterMap = interpreterMap;
            _reportTracker = reportTracker;
        }

        public async Task<BehaviorTreeState> Tick(
            ActorBehaviorTreeShape shape,
            IObjectEntity actor
        )
        {
            var treeState = GetActorState(
                shape,
                actor
            ).Report(
                "Kernel Tick START"
            ).PopActiveNodeFromQueue();
            do
            {
                // Run the state through the Interperters.
                treeState = await _interpreterMap.InterperterByType(
                    treeState.ActiveNode.Type
                ).Run(
                    actor,
                    treeState
                );

                while (treeState.CheckTraversal)
                {
                    treeState = treeState.SetCheckTraversal(
                        false
                    ).ActivateNode(
                        treeState.ActiveTraversal.Token
                    );
                    treeState = await _interpreterMap.InterperterByType(
                        treeState.ActiveTraversal.Type
                    ).Run(
                        actor,
                        treeState
                    );
                }
            } while (treeState.ContainsNext);
            return treeState.Report(
                "Kernel Tick ENDING"
            );
        }

        private BehaviorTreeState GetActorState(
            ActorBehaviorTreeShape shape,
            IObjectEntity actor
        )
        {
            var treeState = actor.GetProperty<BehaviorTreeState>(
                BehaviorTreeState.PROPERTY_NAME
            );
            if (!treeState.IsValid)
            {
                treeState = new BehaviorTreeState(
                    shape
                );
            }
            return treeState.SetReportTracker(
                $"{actor.Id}_{actor.Name}",
                _reportTracker
            ).SetShape(
                shape
            );
        }
    }
}
