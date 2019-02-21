using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.SkillFile
{
    public struct GetSystemCombatSkillScriptsFileHandler : IRequestHandler<GetSystemCombatSkillScriptsFileEvent, SystemCombatSkillScriptsFile>
    {
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;

        public GetSystemCombatSkillScriptsFileHandler(
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader
        )
        {
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
        }
        public async Task<SystemCombatSkillScriptsFile> Handle(GetSystemCombatSkillScriptsFileEvent request, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(
                _serverInfo.AssetsPath,
                "Config",
                request.FileName
            );
            return await _fileLoader.GetFile<SystemCombatSkillScriptsFile>(filePath);
        }
    }
}