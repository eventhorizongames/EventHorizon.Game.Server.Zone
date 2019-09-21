
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.System.Combat.Level.Upgrade
{
    public interface IUpgradePropertyLevel
    {
        LevelStateUpgradeResponse Upgrade(IObjectEntity entity);
    }
}