namespace EventHorizon.Zone.System.Server.Scripts.Register
{
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Server.Scripts.Events.Register;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using MediatR;

    public class SystemServerScriptRegisteredEventHandler 
        : INotificationHandler<ServerScriptRegisteredEvent>
    {
        private readonly ServerScriptDetailsRepository _serverScriptDetailsRepository;

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
                    request.TagList
                )
            );
            return Unit.Task;
        }
    }
}