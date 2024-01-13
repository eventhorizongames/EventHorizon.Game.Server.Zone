namespace EventHorizon.Zone.System.Combat.Level.Upgrade.Property;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Model;

public class UpgradeAttackLevel
    : IUpgradePropertyLevel
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
    private bool EntityHasAvailableExperiencePointsForUpgrade(long entityExperience, long requiredExperience)
    {
        return entityExperience < requiredExperience;
    }

    private long RequiredExperiencePointsForUpgrade(long level)
    {
        return level * 100;
    }
}
