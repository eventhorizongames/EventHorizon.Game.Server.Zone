using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Validators
{
    public class TargetInRangeValidator : IValidator
    {
        public string Id { get; } = "target_not_caster";
        public bool check(IObjectEntity caster, IObjectEntity target, Dictionary<string, object> data)
        {
            var distance = Vector3.Distance(
                caster.Position.CurrentPosition,
                target.Position.CurrentPosition
            );
            var min = (int)data["min"];
            var max = (int)data["max"];
            return min <= distance && distance >= max;
        }
    }
}