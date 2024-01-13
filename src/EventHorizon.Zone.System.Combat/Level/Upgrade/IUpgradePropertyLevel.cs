namespace EventHorizon.Zone.System.Combat.Level.Upgrade;

using EventHorizon.Zone.Core.Model.Entity;

public interface IUpgradePropertyLevel
{
    LevelStateUpgradeResponse Upgrade(IObjectEntity entity);
}
