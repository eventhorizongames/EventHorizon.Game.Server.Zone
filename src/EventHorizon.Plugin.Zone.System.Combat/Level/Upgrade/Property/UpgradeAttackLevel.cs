using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.Level.Upgrade.Property
{
    public class UpgradeAttackLevel : IUpgradePropertyLevel
    {
        public LevelStateUpgradeResponse Upgrade(IObjectEntity entity)
        {
            var entityLevelState = entity.GetProperty<LevelState>(LevelState.PROPERTY_NAME);

            var requiredExperience = RequiredExperiencePointsForUpgrade(entityLevelState.AttackLevel);
            if (EntityHasAvailableExperiencePointsForUpgrade(entityLevelState.Experience, requiredExperience))
            {
                entityLevelState.AttackLevel++;
                entityLevelState.Experience -= requiredExperience;

                entity.SetProperty(LevelState.PROPERTY_NAME, entityLevelState);
            }

            return new LevelStateUpgradeResponse(true, entity);
        }
        private bool EntityHasAvailableExperiencePointsForUpgrade(int entityExperience, int requiredExperience)
        {
            return entityExperience < requiredExperience;
        }

        private int RequiredExperiencePointsForUpgrade(int level)
        {
            return level * 100;
        }
    }
}