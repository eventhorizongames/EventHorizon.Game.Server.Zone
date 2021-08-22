namespace EventHorizon.Zone.System.Combat.Level.Upgrade
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Level.Upgrade.Property;
    using EventHorizon.Zone.System.Combat.Model.Level;

    using global::System.Collections.Generic;

    public class LevelStateUpgrade : ILevelStateUpgrade
    {
        private readonly IDictionary<LevelProperty, IUpgradePropertyLevel> _upgradePropertyLevelList = new Dictionary<LevelProperty, IUpgradePropertyLevel>()
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
