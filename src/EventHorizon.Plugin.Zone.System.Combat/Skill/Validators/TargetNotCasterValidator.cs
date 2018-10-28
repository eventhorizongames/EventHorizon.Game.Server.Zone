using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Validators
{
    public class TargetNotCasterValidator : IValidator
    {
        public string Id { get; } = "target_not_caster";
        public bool check(IObjectEntity caster, IObjectEntity target, Dictionary<string, object> data)
        {
            return caster.Id != target.Id;
        }
    }
}