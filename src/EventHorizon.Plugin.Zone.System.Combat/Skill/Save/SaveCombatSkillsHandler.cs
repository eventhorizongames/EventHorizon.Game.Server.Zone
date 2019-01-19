using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Skill.SkillFile;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Save
{
    public struct SaveCombatSkillsHandler : INotificationHandler<SaveCombatSkillsEvent>
    {
        readonly IMediator _mediator;
        readonly ISkillRepository _skillRepository;
        public SaveCombatSkillsHandler(
            IMediator mediator,
            ISkillRepository skillRepository
        )
        {
            _mediator = mediator;
            _skillRepository = skillRepository;
        }
        public Task Handle(SaveCombatSkillsEvent notification, CancellationToken cancellationToken)
        {
            return _mediator.Publish(new SaveCombatSkillsToFileEvent
            {
                SkillList = _skillRepository.All()
            });
        }
    }
}