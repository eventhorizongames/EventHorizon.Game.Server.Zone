using System.Collections.Generic;
using EventHorizon.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.Fetch
{
    public struct GetAllCombatSkillsQuery : IRequest<IList<SkillInstance>>
    {
        
    }
}