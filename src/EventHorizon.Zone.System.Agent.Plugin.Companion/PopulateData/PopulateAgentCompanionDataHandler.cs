namespace EventHorizon.Zone.System.Agent.Plugin.Companion.PopulateData;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.PopulateData;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class PopulateAgentCompanionDataHandler
    : INotificationHandler<PopulateAgentEntityDataEvent>
{
    private static CompanionState DEFAULT_COMPANION_STATE = new()
    {
        DefaultBehaviorTreeId = "DEFAULT",
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
        agent.PopulateData(
            CompanionState.PROPERTY_NAME,
            DEFAULT_COMPANION_STATE
        );

        return Task.CompletedTask;
    }
}
