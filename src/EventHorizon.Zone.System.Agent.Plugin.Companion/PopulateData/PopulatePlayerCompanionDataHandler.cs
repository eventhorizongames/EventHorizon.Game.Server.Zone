namespace EventHorizon.Zone.System.Agent.Plugin.Companion.PopulateData
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;
    using EventHorizon.Zone.Core.Model.Entity;
    using MediatR;
    using EventHorizon.Zone.Core.Events.Entity.Data;

    public class PopulatePlayerCompanionDataHandler : INotificationHandler<PopulateEntityDataEvent>
    {
        private static CompanionState DEFAULT_COMPANION_STATE = new CompanionState
        {
            DefaultBehaviorTreeId = "$DEFAULT$SHAPE.json",
        };

        public Task Handle(
            PopulateEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var entity = request.Entity;

            if (entity.Type != EntityType.PLAYER)
            {
                return Task.CompletedTask;
            }

            // Populate the Companion Management State on the Player.
            entity.PopulateData<CompanionManagementState>(
                CompanionManagementState.PROPERTY_NAME
            );

            return Task.CompletedTask;
        }
    }
}