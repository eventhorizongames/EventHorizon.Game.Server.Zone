using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Level.Upgrade.Property;
using EventHorizon.Plugin.Zone.System.Combat.Model.Level;

namespace EventHorizon.Plugin.Zone.System.Combat.Level.Upgrade
{
    public class LevelStateUpgrade : ILevelStateUpgrade
    {
        private static IDictionary<LevelProperty, IUpgradePropertyLevel> _upgradePropertyLevelList = new Dictionary<LevelProperty, IUpgradePropertyLevel>()
        {
            {
                LevelProperty.HP,
                new UpgradeHealthPointLevel()
            },
            {
                LevelProperty.AP,
                new UpgradeActionPointLevel()
            },
            {
                LevelProperty.ATTACK,
                new UpgradeAttackLevel()
            },
        };

        public LevelStateUpgradeResponse Upgrade(IObjectEntity entity, LevelProperty property)
        {
            return _upgradePropertyLevelList[property].Upgrade(entity);
        }
    }
}