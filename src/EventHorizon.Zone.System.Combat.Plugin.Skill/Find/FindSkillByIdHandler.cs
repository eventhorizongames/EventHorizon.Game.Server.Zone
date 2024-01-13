namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Find;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.State;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class FindSkillByIdHandler
    : IRequestHandler<FindSkillByIdEvent, SkillInstance>
{
    private readonly SkillRepository _skillRepository;

    public FindSkillByIdHandler(
        SkillRepository skillRepository
    )
    {
        _skillRepository = skillRepository;
    }

    public Task<SkillInstance> Handle(
        FindSkillByIdEvent request,
        CancellationToken cancellationToken
    ) => _skillRepository.Find(
        request.SkillId
    ).FromResult();
}
