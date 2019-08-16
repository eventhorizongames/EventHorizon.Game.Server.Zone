using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Save
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
            // TODO: Save the skill details right to file system
            // Set is not needed, saving to file system will automatically triggered reload on changes.
            _skillRepository.Set(
                request.Skill
            );
            return Task.FromResult(
                request.Skill
            );
        }
    }
}