namespace EventHorizon.Zone.System.Combat.Plugin.Skill.PopulateData
{
    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class PopulateEntityDataForSkillHandler
        : INotificationHandler<PopulateEntityDataEvent>
    {
        public Task Handle(
            PopulateEntityDataEvent notification,
            CancellationToken cancellationToken
        )
        {
            var entity = notification.Entity;

            entity.PopulateData(
                SkillState.PROPERTY_NAME,
                SkillState.NEW
            );

            ValidateSkillState(
                entity
            );

            return Task.CompletedTask;
        }

        private void ValidateSkillState(
            IObjectEntity entity
        )
        {
            var skillState = entity.GetProperty<SkillState>(
                SkillState.PROPERTY_NAME
            );

            if (skillState.SkillMap.List == null)
            {
                entity.SetProperty(
                    SkillState.PROPERTY_NAME,
                    SkillState.NEW
                );
            }
        }
    }
}
