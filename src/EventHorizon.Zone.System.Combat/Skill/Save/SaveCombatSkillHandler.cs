using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Skill.Model;
using EventHorizon.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.Save
{
    public struct SaveCombatSkillHandler : IRequestHandler<SaveCombatSkillEvent, SkillInstance>
    {
        readonly ISkillRepository _skillRepository;
        public SaveCombatSkillHandler(
            ISkillRepository skillRepository
        )
        {
            _skillRepository = skillRepository;
        }
        public Task<SkillInstance> Handle(SaveCombatSkillEvent request, CancellationToken cancellationToken)
        {
            _skillRepository.Set(
                request.Skill
            );
            return Task.FromResult(
                request.Skill
            );
        }
    }
}