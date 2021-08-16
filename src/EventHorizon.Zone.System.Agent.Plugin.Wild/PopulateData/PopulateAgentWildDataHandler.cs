namespace EventHorizon.Zone.System.Agent.Plugin.Wild.PopulateData
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.PopulateData;
    using EventHorizon.Zone.System.Agent.Plugin.Wild.Model;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class PopulateAgentWildDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        public Task Handle(
            PopulateAgentEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var agent = request.Agent;

            // Populate the Agent's Wild State from Data.
            agent.PopulateData<AgentWildState>(
                AgentWildState.PROPERTY_NAME,
                AgentWildState.NEW
            );

            return Task.CompletedTask;
        }
    }
}
