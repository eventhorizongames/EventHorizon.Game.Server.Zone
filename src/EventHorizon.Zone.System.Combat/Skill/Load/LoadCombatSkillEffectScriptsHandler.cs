using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.Load
{
    public class LoadCombatSkillEffectScriptsHandler : IRequestHandler<LoadCombatSkillEffectScripts>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly SystemProvidedAssemblyList _systemAssemblyList;

        public LoadCombatSkillEffectScriptsHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            SystemProvidedAssemblyList systemAssemblyList
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _systemAssemblyList = systemAssemblyList;
        }

        public Task<Unit> Handle(
            LoadCombatSkillEffectScripts notification,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new LoadFileRecursivelyFromDirectory(
                Path.Combine(
                    _serverInfo.ServerScriptsPath,
                    "Effects"
                ),
                OnProcessFile,
                new Dictionary<string, object>
                {
                    {
                        "RootPath",
                        _serverInfo.ServerScriptsPath
                    },
                    {
                        "ScriptReferenceAssemblies",
                        _systemAssemblyList.List
                    },
                    {
                        "ScriptImports",
                        new string[] { }
                    }
                }
            )
        );

        private async Task OnProcessFile(
            StandardFileInfo fileInfo,
            IDictionary<string, object> arguments
        )
        {
            var rootPath = arguments["RootPath"] as string;
            var scriptReferenceAssemblies = arguments["ScriptReferenceAssemblies"] as IList<Assembly>;
            var scriptImports = arguments["ScriptImports"] as string[];
            var tagList = new string[]
            {
                "Type:SkillEffectScript"
            };
            // Register Script with Platform
            await _mediator.Send(
                new RegisterServerScriptCommand(
                    fileInfo.Name,
                    rootPath.MakePathRelative(
                        fileInfo.DirectoryName
                    ),
                    await _mediator.Send(
                        new ReadAllTextFromFile(
                            fileInfo.FullName
                        )
                    ),
                    scriptReferenceAssemblies,
                    scriptImports,
                    tagList
                )
            );
        }
    }
}