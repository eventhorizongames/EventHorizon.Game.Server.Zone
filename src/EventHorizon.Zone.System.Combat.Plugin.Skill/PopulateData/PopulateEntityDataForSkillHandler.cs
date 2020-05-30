using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Data;
using MediatR;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;

namespace EventHorizon.Zone.System.Combat.PopulateData
{
    public class PopulateEntityDataForSkillHandler : INotificationHandler<PopulateEntityDataEvent>
    {
        public Task Handle(
            PopulateEntityDataEvent notification, 
            CancellationToken cancellationToken
        )
        {
            var entity = notification.Entity;

            entity.PopulateData<SkillState>(
                SkillState.PROPERTY_NAME, 
                SkillState.NEW
            );

            this.ValidateSkillState(
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