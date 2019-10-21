using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Plugin.Editor.Skills.Model;
using EventHorizon.Zone.System.Combat.Skill.State;
using EventHorizon.Zone.System.Server.Scripts.Events.Query;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Plugin.Editor.Skills.Query
{
    public struct EditorSystemCombatSkillScriptsFileQueryHandler : IRequestHandler<EditorSystemCombatSkillScriptsFileQuery, EditorSystemCombatSkillScriptsFile>
    {
        readonly IMediator _mediator;
        readonly ISkillValidatorScriptRepository _validatorScriptRepository;

        public EditorSystemCombatSkillScriptsFileQueryHandler(
            IMediator mediator,
            ISkillValidatorScriptRepository validatorScriptRepository
        )
        {
            _mediator = mediator;
            _validatorScriptRepository = validatorScriptRepository;
        }

        public async Task<EditorSystemCombatSkillScriptsFile> Handle(
            EditorSystemCombatSkillScriptsFileQuery request,
            CancellationToken cancellationToken
        )
        {
            return new EditorSystemCombatSkillScriptsFile(
                await _mediator.Send(
                    new QueryForServerScriptDetails(
                        script => script.TagList.Contains(
                            "Type:SkillEffectScript"
                        )
                    )
                ),
                _validatorScriptRepository.All()
            );
        }
    }
}