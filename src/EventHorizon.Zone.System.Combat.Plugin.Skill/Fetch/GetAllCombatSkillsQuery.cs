using System.Collections.Generic;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Fetch
{
    public struct GetAllCombatSkillsQuery : IRequest<IList<SkillInstance>>
    {
        
    }
}