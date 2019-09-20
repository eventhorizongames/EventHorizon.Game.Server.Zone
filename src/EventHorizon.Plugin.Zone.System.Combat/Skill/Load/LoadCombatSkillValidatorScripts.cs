using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Extensions;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Load
{
    public struct LoadCombatSkillValidatorScripts : IRequest
    {
        public struct LoadCombatSkillValidatorScriptsHandler : IRequestHandler<LoadCombatSkillValidatorScripts>
        {
            readonly IMediator _mediator;
            readonly ServerInfo _serverInfo;
            readonly ISkillValidatorScriptRepository _skillValidatorScriptRepository;

            public LoadCombatSkillValidatorScriptsHandler(
                IMediator mediator,
                ServerInfo serverInfo,
                ISkillValidatorScriptRepository skillValidatorScriptRepository
            )
            {
                _mediator = mediator;
                _serverInfo = serverInfo;
                _skillValidatorScriptRepository = skillValidatorScriptRepository;
            }
            public Task<Unit> Handle(
                LoadCombatSkillValidatorScripts request,
                CancellationToken cancellationToken
            )
            {
                this.LoadFromDirectoryInfo(
                    Path.Combine(
                        _serverInfo.ServerPath,
                        "Scripts"
                    ),
                    new DirectoryInfo(
                        Path.Combine(
                            _serverInfo.ServerPath,
                            "Scripts",
                            "Validators"
                        )
                    )
                );

                return Unit.Task;
            }
            private void LoadFromDirectoryInfo(
                string scriptsPath,
                DirectoryInfo directoryInfo
            )
            {
                // Load Scripts from Sub-Directories
                foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
                {
                    // Load Files From Directories
                    this.LoadFromDirectoryInfo(
                        scriptsPath,
                        subDirectoryInfo
                    );
                }
                // Load script files into Repository
                this.LoadFileIntoRepository(
                    scriptsPath,
                    directoryInfo
                );
            }
            private void LoadFileIntoRepository(
                string scriptsPath,
                DirectoryInfo directoryInfo
            )
            {
                foreach (var validatorFile in directoryInfo.GetFiles())
                {
                    _skillValidatorScriptRepository.Add(
                        SkillValidatorScript.CreateScript(
                            GenerateName(
                                $"{scriptsPath}{Path.DirectorySeparatorChar}".MakePathRelative(
                                    validatorFile.DirectoryName
                                ), validatorFile.Name
                            ),
                            File.ReadAllText(
                                validatorFile.FullName
                            )
                        )
                    );
                }
            }

            private static string GenerateName(
                string path,
                string fileName
            )
            {
                return string.Join(
                    "_",
                    string.Join(
                        "_",
                        path.Split(
                            Path.DirectorySeparatorChar
                        )
                    ),
                    fileName
                );
            }
        }
    }
}