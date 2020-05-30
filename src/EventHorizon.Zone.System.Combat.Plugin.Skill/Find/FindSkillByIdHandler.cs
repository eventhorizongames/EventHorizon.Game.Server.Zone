using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.State;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Find
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