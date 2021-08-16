namespace EventHorizon.Game.Registered
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.Model;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;

    using MediatR;

    public class CheckSkillsEntityRegisteredHandler
        : INotificationHandler<EntityRegisteredEvent>
    {
        public Task Handle(
            EntityRegisteredEvent notification,
            CancellationToken cancellationToken
        )
        {
            var entity = notification.Entity;
            if (entity.Type != EntityType.PLAYER)
            {
                return Task.CompletedTask;
            }

            var skillState = entity.GetProperty<SkillState>(
                SkillState.PROPERTY_NAME
            );
            if (!skillState.SkillMap.Contains(
                SkillConstants.ESCAPE_OF_CAPTURES_SKILL_ID
            ))
            {
                skillState = skillState.SetSkill(
                    new SkillStateDetails
                    {
                        Id = SkillConstants.ESCAPE_OF_CAPTURES_SKILL_ID,
                    }
                );
                entity.SetProperty(
                    SkillState.PROPERTY_NAME,
                    skillState
                );
            }

            return Task.CompletedTask;
        }
    }
}
