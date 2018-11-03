using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Fetch
{
    public class FetchAllSkillActionScriptListHandler : IRequestHandler<FetchAllSkillActionScriptListEvent, IEnumerable<SkillActionScript>>
    {
        readonly ISkillActionScriptRepository _skillActionScriptRepository;
        public FetchAllSkillActionScriptListHandler(
            ISkillActionScriptRepository skillActionScriptRepository
        )
        {
            _skillActionScriptRepository = skillActionScriptRepository;
        }
        public Task<IEnumerable<SkillActionScript>> Handle(FetchAllSkillActionScriptListEvent request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _skillActionScriptRepository.All()
            );
        }
    }
}