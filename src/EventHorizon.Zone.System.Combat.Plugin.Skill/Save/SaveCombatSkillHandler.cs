namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Save
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;
    using MediatR;

    public class SaveCombatSkillHandler
        : IRequestHandler<SaveCombatSkillEvent, SkillInstance>
    {
        private readonly SkillRepository _skillRepository;

        public SaveCombatSkillHandler(
            SkillRepository skillRepository
        )
        {
            _skillRepository = skillRepository;
        }

        public Task<SkillInstance> Handle(
            SaveCombatSkillEvent request,
            CancellationToken cancellationToken
        )
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