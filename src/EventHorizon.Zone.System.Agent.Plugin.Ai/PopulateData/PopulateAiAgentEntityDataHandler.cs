namespace EventHorizon.Zone.System.Agent.Plugin.Ai.PopulateData
{
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.PopulateData;
    using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;

    using MediatR;

    public class PopulateAgentEntityDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        public Task Handle(
            PopulateAgentEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var agent = request.Agent;

            agent.PopulateData<AgentWanderState>(
                AgentWanderState.WANDER_NAME
            );

            return Task.CompletedTask;
        }
    }
}
