using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Editor.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Editor.Skills.Query
{
    public struct EditorSystemCombatSkillScriptsFileQueryHandler : IRequestHandler<
    EventHorizon.Plugin.Zone.System.Combat.Editor.Skills.Query.EditorSystemCombatSkillScriptsFileQuery, EventHorizon.Plugin.Zone.System.Combat.Editor.Model.EditorSystemCombatSkillScriptsFile>
    {
        readonly ISkillEffectScriptRepository _effectScriptRepository;
        readonly ISkillActionScriptRepository _actionScriptRepository;
        readonly ISkillValidatorScriptRepository _validatorScriptRepository;

        public EditorSystemCombatSkillScriptsFileQueryHandler(
            ISkillEffectScriptRepository effectScriptRepository,
            ISkillActionScriptRepository actionScriptRepository,
            ISkillValidatorScriptRepository validatorScriptRepository
        )
        {
            _effectScriptRepository = effectScriptRepository;
            _actionScriptRepository = actionScriptRepository;
            _validatorScriptRepository = validatorScriptRepository;
        }

        public Task<EditorSystemCombatSkillScriptsFile> Handle(
            EditorSystemCombatSkillScriptsFileQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                new EditorSystemCombatSkillScriptsFile(
                    _effectScriptRepository.All(),
                    _actionScriptRepository.All(),
                    _validatorScriptRepository.All()
                )
            );
        }
    }
}