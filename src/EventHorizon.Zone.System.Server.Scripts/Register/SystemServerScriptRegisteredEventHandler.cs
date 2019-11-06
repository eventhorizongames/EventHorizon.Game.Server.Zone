using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;
using EventHorizon.Zone.System.Server.Scripts.State;
using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.Register
{
    public class SystemServerScriptRegisteredEventHandler : INotificationHandler<ServerScriptRegisteredEvent>
    {
        readonly ServerScriptDetailsRepository _serverScriptDetailsRepository;

        public SystemServerScriptRegisteredEventHandler(
            ServerScriptDetailsRepository serverScriptDetailsRepository
        )
        {
            _serverScriptDetailsRepository = serverScriptDetailsRepository;
        }

        public Task Handle(
            ServerScriptRegisteredEvent request,
            CancellationToken cancellationToken
        )
        {
            _serverScriptDetailsRepository.Add(
                request.Id,
                new ServerScriptDetails(
                    request.Id,
                    request.FileName,
                    request.Path,
                    request.ScriptString,
                    request.ReferenceAssemblies.Select(
                        assembly => assembly.ToString()
                    ),
                    
                    request.Imports,
                    request.TagList
                )
            );
            return Unit.Task;
        }
    }
}