namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Query
{
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Model;
    using EventHorizon.Zone.System.Server.Scripts.Events.Query;

    using MediatR;

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
                    ),
                    cancellationToken
                ),
                await _mediator.Send(
                    new QueryForServerScriptDetails(
                        script => script.TagList.Contains(
                            "Type:SkillValidatorScript"
                        )
                    ),
                    cancellationToken
                )
            );
        }
    }
}
