using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;
using EventHorizon.Zone.System.Agent.Events.PopulateData;

namespace EventHorizon.Zone.System.Agent.Plugin.Companion.PopulateData
{
    public class PopulateAgentCompanionDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        private static CompanionState DEFAULT_COMPANION_STATE = new CompanionState
        {
            DefaultBehaviorTreeId = "$DEFAULT$SHAPE.json",
        };

        public Task Handle(
            PopulateAgentEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var agent = request.Agent;

            // Populates the Owner State on the Agent from loaded data.
            agent.PopulateData<OwnerState>(
                OwnerState.PROPERTY_NAME
            );

            // Populates the Companion State on the Agent from loaded data.
            agent.PopulateData<CompanionState>(
                CompanionState.PROPERTY_NAME,
                DEFAULT_COMPANION_STATE
            );

            return Task.CompletedTask;
        }
    }
}