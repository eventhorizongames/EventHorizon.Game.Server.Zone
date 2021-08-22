namespace EventHorizon.Zone.System.Combat.Life
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Life.Change;
    using EventHorizon.Zone.System.Combat.Life.Change.Property;
    using EventHorizon.Zone.System.Combat.Model.Life;

    using global::System.Collections.Generic;

    public class LifeStateChange
        : ILifeStateChange
    {
        private readonly IDictionary<string, IChangeLifeProperty> _changeLifePropertyList = new Dictionary<string, IChangeLifeProperty>()
        {
            {
                LifeProperty.HP,
                new ChangeHealthPoints()
            },
            {
                LifeProperty.AP,
                new ChangeActionPoints()
            },
        };

        public LifeStateChangeResponse Change(IObjectEntity entity, string property, long points)
        {
            return _changeLifePropertyList[property].Change(entity, points);
        }
    }
}
