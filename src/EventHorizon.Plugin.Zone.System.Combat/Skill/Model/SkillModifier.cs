using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SkillModifier
    {
        public string modifier { get; set; }
        public string propertyName { get; set; }
        public string valueProperty { get; set; }
        public long value { get; set; }

        public IObjectEntity apply(IObjectEntity target)
        {
            var property = target.GetProperty<dynamic>(valueProperty);
            var propertyValue = (long)property[valueProperty];
            property[valueProperty] = propertyValue + value;
            return target;
        }
    }
}