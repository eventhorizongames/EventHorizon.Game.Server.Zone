using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.System;
using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.Register
{
    public class SystemRegisterServerScriptCommandHandler : IRequestHandler<RegisterServerScriptCommand>
    {
        readonly IMediator _mediator;
        readonly ServerScriptRepository _serverScriptRepository;

        public SystemRegisterServerScriptCommandHandler(
            IMediator mediator,
            ServerScriptRepository serverScriptRepository
        )
        {
            _mediator = mediator;
            _serverScriptRepository = serverScriptRepository;
        }

        public async Task<Unit> Handle(
            RegisterServerScriptCommand request,
            CancellationToken cancellationToken
        )
        {
            var script = SystemServerScript.Create(
                request.FileName,
                request.Path,
                request.ScriptString,
                request.ReferenceAssemblies,
                request.Imports
            );
            _serverScriptRepository.Add(
                script
            );
            await _mediator.Publish(
                new ServerScriptRegisteredEvent(
                    script.Id,
                    request.FileName,
                    request.Path,
                    request.ScriptString,
                    request.ReferenceAssemblies,
                    request.Imports,
                    request.TagList
                )
            );
            return Unit.Value;
        }
    }
}