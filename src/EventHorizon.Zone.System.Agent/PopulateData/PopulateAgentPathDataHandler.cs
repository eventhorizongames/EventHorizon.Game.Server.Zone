namespace EventHorizon.Zone.System.Agent.PopulateData
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Entity;
    using MediatR;
    using EventHorizon.Zone.System.Agent.Events.PopulateData;
    using EventHorizon.Zone.System.Agent.Model.Path;

    public class PopulateAgentPathDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        public Task Handle(
            PopulateAgentEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var agent = request.Agent;

            // Populate the Agent with a New Path State.
            agent.SetProperty(
                PathState.PROPERTY_NAME,
                PathState.NEW
            );

            return Task.CompletedTask;
        }
    }
}