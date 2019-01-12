using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.Model;
using EventHorizon.Game.Server.Zone.Agent.Ai.State;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.LoadRoutine
{
    public struct LoadAgentRoutineSystemHandler : INotificationHandler<LoadAgentRoutineSystemEvent>
    {
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly IAgentRoutineRepository _agentRoutineRepository;

        public LoadAgentRoutineSystemHandler(
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            IAgentRoutineRepository agentRoutineRepository
        )
        {
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _agentRoutineRepository = agentRoutineRepository;
        }

        public async Task Handle(LoadAgentRoutineSystemEvent notification, CancellationToken cancellationToken)
        {
            foreach (var routine in await GetAgentRoutineList())
            {
                _agentRoutineRepository.Add(
                    routine.CreateScript(
                        GetRoutineScriptsPath()
                    )
                );
            }
        }
        private string GetRoutineScriptsPath()
        {
            return Path.Combine(
                _serverInfo.ScriptsPath,
                "Routines"
            );
        }
        private async Task<IEnumerable<AgentRoutineScript>> GetAgentRoutineList()
        {
            return (await GetAgentRoutineSystemFile()).RoutineList;
        }
        private Task<AgentRoutineFile> GetAgentRoutineSystemFile()
        {
            return _fileLoader.GetFile<AgentRoutineFile>(
                Path.Combine(_serverInfo.SystemsPath, "System.Agent.Routine.json")
            );
        }
    }
}