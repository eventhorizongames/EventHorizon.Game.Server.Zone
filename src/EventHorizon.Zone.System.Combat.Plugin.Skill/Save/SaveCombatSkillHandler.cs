namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Save
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;
    using global::System.Threading;
    using global::System.Threading.Tasks;
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