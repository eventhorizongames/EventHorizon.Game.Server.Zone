using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Model.Level;

namespace EventHorizon.Plugin.Zone.System.Combat.Level
{
    public interface ILevelStateUpgrade
    {
        LevelStateUpgradeResponse Upgrade(IObjectEntity entity, LevelProperty property);
    }
    public struct LevelStateUpgradeResponse
    {
        public bool Success { get; }
        public IObjectEntity ChangedEntity { get; }
        public LevelStateUpgradeResponse(
            bool success,
            IObjectEntity changedEntity
        )
        {
            this.Success = success;
            ChangedEntity = changedEntity;
        }
    }
}