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

        public async Task Handle(
            LoadAgentRoutineSystemEvent notification,
            CancellationToken cancellationToken
        )
        {
            // Get a list of files from directory
            var directory = new DirectoryInfo(GetRoutinesPath());
            foreach (var routineFile in directory.GetFiles())
            {
                var routine = await _fileLoader.GetFile<AgentRoutineScript>(routineFile.FullName);

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
                _serverInfo.ServerPath,
                "Scripts",
                "Routines"
            );
        }
        private string GetRoutinesPath()
        {
            return Path.Combine(
                _serverInfo.ServerPath,
                "Routines"
            );
        }
    }
}