using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Validators
{
    public class CasterHasCostApValidator : IValidator
    {
        public string Id { get; } = "caster_has_cost";
        public bool check(IObjectEntity caster, IObjectEntity target, Dictionary<string, object> data)
        {
            var propertyName = (string)data["propertyName"];
            var valueProperty = (string)data["valueProperty"];
            var cost = (int)data["cost"];
            var propertyValue = caster.GetProperty<dynamic>(propertyName)[valueProperty];
            return propertyValue >= cost;
        }
    }
}