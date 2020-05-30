using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.State;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Save
{
    public class SaveCombatSkillHandler : IRequestHandler<SaveCombatSkillEvent, SkillInstance>
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