using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Skill.Model;
using EventHorizon.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.Fetch
{
    public struct GetAllCombatSkillsQueryHandler : IRequestHandler<GetAllCombatSkillsQuery, IList<SkillInstance>>
    {
        readonly ISkillRepository _skillRepository;
        public GetAllCombatSkillsQueryHandler(
            ISkillRepository skillRepository
        )
        {
            _skillRepository = skillRepository;
        }
        public Task<IList<SkillInstance>> Handle(
            GetAllCombatSkillsQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _skillRepository.All()
            );
        }
    }
}