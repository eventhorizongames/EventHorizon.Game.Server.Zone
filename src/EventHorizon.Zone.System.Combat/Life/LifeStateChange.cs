using System;
using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Life.Change;
using EventHorizon.Zone.System.Combat.Life.Change.Property;
using EventHorizon.Zone.System.Combat.Model.Life;

namespace EventHorizon.Zone.System.Combat.Life
{
    public class LifeStateChange : ILifeStateChange
    {
        private static IDictionary<string, IChangeLifeProperty> _changeLifePropertyList = new Dictionary<string, IChangeLifeProperty>()
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
