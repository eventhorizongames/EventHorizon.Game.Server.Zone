using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Find
{
    public class FindSkillByIdHandler : IRequestHandler<FindSkillByIdEvent, SkillInstance>
    {
        readonly ISkillRepository _skillRepository;

        public FindSkillByIdHandler(
            ISkillRepository skillRepository
        )
        {
            _skillRepository = skillRepository;
        }
        public Task<SkillInstance> Handle(FindSkillByIdEvent request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_skillRepository.Find(
                request.SkillId
            ));
        }
    }
}