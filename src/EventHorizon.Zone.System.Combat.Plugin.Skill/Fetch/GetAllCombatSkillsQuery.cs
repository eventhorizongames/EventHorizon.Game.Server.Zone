namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Fetch
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

    using global::System.Collections.Generic;

    using MediatR;

    public struct GetAllCombatSkillsQuery : IRequest<IList<SkillInstance>>
    {

    }
}
