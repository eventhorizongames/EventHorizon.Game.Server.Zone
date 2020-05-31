namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Fetch
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class GetAllCombatSkillsQueryHandler
        : IRequestHandler<GetAllCombatSkillsQuery, IList<SkillInstance>>
    {
        private readonly SkillRepository _skillRepository;

        public GetAllCombatSkillsQueryHandler(
            SkillRepository skillRepository
        )
        {
            _skillRepository = skillRepository;
        }

        public Task<IList<SkillInstance>> Handle(
            GetAllCombatSkillsQuery request,
            CancellationToken cancellationToken
        ) => _skillRepository.All().FromResult();
    }
}