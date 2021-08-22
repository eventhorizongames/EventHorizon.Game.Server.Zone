namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model
{
    using EventHorizon.Zone.Core.Model.Entity;

    using global::System.Collections.Generic;

    public interface IValidator
    {
        string Id { get; }
        bool check(IObjectEntity caster, IObjectEntity target, Dictionary<string, object> data);
    }
}
