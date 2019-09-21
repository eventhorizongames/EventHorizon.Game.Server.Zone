using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.System.Combat.Skill.Model
{
    public interface IValidator
    {
        string Id { get; }
        bool check(IObjectEntity caster, IObjectEntity target, Dictionary<string, object> data);
    }
}