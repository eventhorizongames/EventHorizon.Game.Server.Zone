namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreter
{
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

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
            ).SetReportTracker(
                $"{actor.Id}_{actor.Name}",
                _reportTracker
            ).PopActiveNodeFromQueue()
            // .ClearReport() // Uncomment this to clear report for clean file.
            .Report(
                "Kernel Tick START"
            );
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
                return new BehaviorTreeState(
                    shape
                );
            }
            return treeState.SetShape(
                shape
            );
        }
    }
}