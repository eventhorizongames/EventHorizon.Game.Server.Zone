using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.SkillFile
{
    public struct GetCombatSkillsFileHandler : IRequestHandler<GetCombatSkillsFileEvent, CombatSkillsFile>
    {
        readonly IJsonFileLoader _fileLoader;
        readonly ServerInfo _serverInfo;
        public GetCombatSkillsFileHandler(
            IJsonFileLoader fileLoader,
            ServerInfo serverInfo
        )
        {
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
        }
        public Task<CombatSkillsFile> Handle(GetCombatSkillsFileEvent request, CancellationToken cancellationToken)
        {
            return _fileLoader.GetFile<CombatSkillsFile>(
                Path.Combine(
                    _serverInfo.AssetsPath,
                    "Config",
                    request.FileName
                )
            );
        }
    }
}