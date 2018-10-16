using System;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using EventHorizon.Plugin.Zone.System.Combat.Model.Life;

namespace EventHorizon.Plugin.Zone.System.Combat.Life.Change.Property
{
    public class ChangeHealthPoints : IChangeLifeProperty
    {
        public LifeStateChangeResponse Change(IObjectEntity entity, int points)
        {
            var entityLifeState = entity.GetProperty<LifeState>(LifeState.PROPERTY_NAME);

            var newHealthPointValue = entityLifeState.HealthPoints + points;
            entityLifeState.HealthPoints += points;

            if (entityLifeState.HealthPoints <= 0)
            {
                entityLifeState.Condition = LifeCondition.DEAD;
            }
            else
            {
                entityLifeState.Condition = LifeCondition.ALIVE;
            }

            if (entityLifeState.HealthPoints > entityLifeState.MaxHealthPoints)
            {
                entityLifeState.HealthPoints = entityLifeState.MaxHealthPoints;
            }

            entity.SetProperty(LifeState.PROPERTY_NAME, entityLifeState);

            return new LifeStateChangeResponse(true, entity);
        }
    }
}