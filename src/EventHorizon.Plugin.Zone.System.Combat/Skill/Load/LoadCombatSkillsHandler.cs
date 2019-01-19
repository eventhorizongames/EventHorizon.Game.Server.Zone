using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.SkillFile;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Load
{
    public class LoadCombatSkillsHandler : INotificationHandler<LoadCombatSkillsEvent>
    {
        readonly IMediator _mediator;
        readonly ISkillRepository _skillRepository;

        public LoadCombatSkillsHandler(
            IMediator mediator,
            IJsonFileLoader fileLoader,
            ISkillRepository skillRepository
        )
        {
            _mediator = mediator;
            _skillRepository = skillRepository;
        }

        public async Task Handle(LoadCombatSkillsEvent notification, CancellationToken cancellationToken)
        {
            var combatSkillsFile = await _mediator.Send(new GetCombatSkillsFileEvent());
            foreach (var skill in combatSkillsFile.SkillList)
            {
                _skillRepository.Add(
                    skill
                );
            }
        }
    }

}