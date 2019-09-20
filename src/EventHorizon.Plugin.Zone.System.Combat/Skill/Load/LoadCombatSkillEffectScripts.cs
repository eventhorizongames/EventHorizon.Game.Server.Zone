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
    public struct LoadCombatSkillEffectScripts : IRequest
    {
        public struct LoadCombatSkillEffectScriptsHandler : IRequestHandler<LoadCombatSkillEffectScripts>
        {
            readonly IMediator _mediator;
            readonly ServerInfo _serverInfo;
            readonly ISkillEffectScriptRepository _skillEffectScriptRepository;

            public LoadCombatSkillEffectScriptsHandler(
                IMediator mediator,
                ServerInfo serverInfo,
                ISkillEffectScriptRepository skillEffectScriptRepository
            )
            {
                _mediator = mediator;
                _serverInfo = serverInfo;
                _skillEffectScriptRepository = skillEffectScriptRepository;
            }
            public Task<Unit> Handle(
                LoadCombatSkillEffectScripts request,
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
                            "Effects"
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
                foreach (var effectFile in directoryInfo.GetFiles())
                {
                    _skillEffectScriptRepository.Add(
                        SkillEffectScript.CreateScript(
                            GenerateName(
                                $"{scriptsPath}{Path.DirectorySeparatorChar}".MakePathRelative(
                                    effectFile.DirectoryName
                                ), effectFile.Name
                            ),
                            File.ReadAllText(
                                effectFile.FullName
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