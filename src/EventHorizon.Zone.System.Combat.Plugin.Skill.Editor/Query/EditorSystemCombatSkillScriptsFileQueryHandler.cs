using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Model;
using EventHorizon.Zone.System.Server.Scripts.Events.Query;

using MediatR;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Query
{
    public class EditorSystemCombatSkillScriptsFileQueryHandler
        : IRequestHandler<EditorSystemCombatSkillScriptsFileQuery, EditorSystemCombatSkillScriptsFile>
    {
        readonly IMediator _mediator;

        public EditorSystemCombatSkillScriptsFileQueryHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
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
                await _mediator.Send(
                    new QueryForServerScriptDetails(
                        script => script.TagList.Contains(
                            "Type:SkillValidatorScript"
                        )
                    )
                )
            );
        }
    }
}
