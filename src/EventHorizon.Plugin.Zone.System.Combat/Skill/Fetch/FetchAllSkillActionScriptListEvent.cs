using System.Collections.Generic;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Fetch
{
    public struct FetchAllSkillActionScriptListEvent : IRequest<IEnumerable<SkillActionScript>>
    {
        
    }
}