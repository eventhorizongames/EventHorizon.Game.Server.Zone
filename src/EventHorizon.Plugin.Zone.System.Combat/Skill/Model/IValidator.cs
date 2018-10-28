using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public interface IValidator
    {
        string Id { get; }
        bool check(IObjectEntity caster, IObjectEntity target, Dictionary<string, object> data);
    }
}