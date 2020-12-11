namespace EventHorizon.Zone.System.Server.Scripts.Register
{
    using EventHorizon.Zone.System.Server.Scripts.Events.Register;
    using EventHorizon.Zone.System.Server.Scripts.Exceptions;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.System;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class SystemRegisterServerScriptCommandHandler : IRequestHandler<RegisterServerScriptCommand>
    {
        private readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly ServerScriptRepository _serverScriptRepository;

        public SystemRegisterServerScriptCommandHandler(
            ILogger<SystemRegisterServerScriptCommandHandler> logger,
            IMediator mediator,
            ServerScriptRepository serverScriptRepository
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverScriptRepository = serverScriptRepository;
        }

        public async Task<Unit> Handle(
            RegisterServerScriptCommand request,
            CancellationToken cancellationToken
        )
        {
            var existing = GetExisting(
                request
            );
            if (existing != default
                && existing.Id == SystemServerScript.GenerateId(
                    request.Path,
                    request.FileName
                ) && existing.Hash == SystemServerScript.GenerateHash(
                    request.ScriptString
                )
            )
            {
                _logger.LogDebug(
                    "Script Already Generated for \nServerScriptCommand: \n | FileName: {FileName} \n | Path: {Path} \n | TagList: {TagList}",
                        request.FileName,
                        request.Path,
                        request.TagList
                );
                // No change to the script, just ignore it.
                return Unit.Value;
            }
            _logger.LogDebug(
                "Loading System Service Script\nServerScriptCommand: \n | FileName: {FileName} \n | Path: {Path} \n | TagList: {TagList}",
                    request.FileName,
                    request.Path,
                    request.TagList
            );
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

        private ServerScript GetExisting(
            RegisterServerScriptCommand request
        )
        {
            try
            {
                return _serverScriptRepository.Find(
                    SystemServerScript.GenerateId(
                        request.Path,
                        request.FileName
                    )
                );
            }
            catch (ServerScriptNotFound)
            {
                return default;
            }
        }
    }
}