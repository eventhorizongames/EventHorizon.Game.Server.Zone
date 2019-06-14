using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Editor.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Editor.Skills.Query
{
    public struct EditorSystemCombatSkillScriptsFileQueryHandler : IRequestHandler<EditorSystemCombatSkillScriptsFileQuery, EditorSystemCombatSkillScriptsFile>
    {
        readonly ISkillEffectScriptRepository _effectScriptRepository;
        readonly ISkillValidatorScriptRepository _validatorScriptRepository;

        public EditorSystemCombatSkillScriptsFileQueryHandler(
            ISkillEffectScriptRepository effectScriptRepository,
            ISkillValidatorScriptRepository validatorScriptRepository
        )
        {
            _effectScriptRepository = effectScriptRepository;
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
                    _validatorScriptRepository.All()
                )
            );
        }
    }
}