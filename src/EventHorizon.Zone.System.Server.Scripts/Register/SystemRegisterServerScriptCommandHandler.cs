using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.System;
using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.Register
{
    public struct SystemRegisterServerScriptCommandHandler : IRequestHandler<RegisterServerScriptCommand>
    {
        readonly ServerScriptRepository _serverScriptRepository;

        public SystemRegisterServerScriptCommandHandler(
            ServerScriptRepository serverScriptRepository
        )
        {
            _serverScriptRepository = serverScriptRepository;
        }

        public Task<Unit> Handle(
            RegisterServerScriptCommand request,
            CancellationToken cancellationToken
        )
        {
            _serverScriptRepository.Add(
                SystemServerScript.Create(
                    request.FileName,
                    request.Path,
                    request.ScriptString,
                    request.ReferenceAssemblies,
                    request.Imports
                )
            );
            return Unit.Task;
        }
    }
}