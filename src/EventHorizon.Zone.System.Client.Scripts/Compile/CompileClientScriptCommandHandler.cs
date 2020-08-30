namespace EventHorizon.Zone.System.Client.Scripts.Compile
{
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::System.Threading;
    using MediatR;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using Microsoft.Extensions.Logging;
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Actions.Reload;
    using EventHorizon.Zone.System.Client.Scripts.Model.Client;

    public class CompileClientScriptCommandHandler
        : IRequestHandler<CompileClientScriptCommand>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ClientScriptsState _state;
        private readonly ClientScriptRepository _clientScriptRepository;
        private readonly ClientScriptCompiler _clientScriptCompiler;

        public CompileClientScriptCommandHandler(
            ILogger<CompileClientScriptCommandHandler> logger,
            IMediator mediator,
            ClientScriptsState state,
            ClientScriptRepository clientScriptRepository,
            ClientScriptCompiler clientScriptCompiler
        )
        {
            _logger = logger;
            _mediator = mediator;
            _state = state;
            _clientScriptRepository = clientScriptRepository;
            _clientScriptCompiler = clientScriptCompiler;
        }

        public async Task<Unit> Handle(
            CompileClientScriptCommand request,
            CancellationToken cancellationToken
        )
        {
            var scripts = _clientScriptRepository.All().Where(
                a => a.ScriptType == Model.ClientScriptType.CSharp
            );
            if (scripts.Any())
            {
                var compilerResult = await _clientScriptCompiler.Compile(
                    scripts
                );
                if (!compilerResult.Success)
                {
                    _logger.LogError(
                        "Failed to compile CSharp Code: {ErrorCode}",
                        compilerResult.ErrorCode
                    );
                    return Unit.Value;
                }
                _state.SetAssembly(
                    compilerResult.Hash,
                    compilerResult.ScriptAssembly
                );

                await _mediator.Publish(
                    ClientScriptsAssemblyChangedClientActionToAllEvent.Create(
                        new ClientScriptsAssemblyChangedClientActionData(
                            compilerResult.Hash,
                            compilerResult.ScriptAssembly
                        )
                    )
                );
            }

            return Unit.Value;
        }
    }
}
