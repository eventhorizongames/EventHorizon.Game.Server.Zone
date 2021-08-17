namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.CSharp
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Assemblies;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Model;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Consolidate;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Create;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class ServerScriptCompilerForCSharp
        : ServerScriptCompiler
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly AssemblyBuilder _builder;

        public ServerScriptCompilerForCSharp(
            ILogger<ServerScriptCompilerForCSharp> logger,
            IMediator mediator,
            ServerInfo serverInfo,
            AssemblyBuilder builder
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
            _builder = builder;
        }

        public async Task<CompiledScriptResult> Compile(
            IEnumerable<ServerScriptDetails> scripts,
            CancellationToken cancellationToken
        )
        {
            var consolidatedScripts = string.Empty;
            try
            {
                var consolidationResult = await _mediator.Send(
                    new ConsolidateServerScriptsCommand(
                        scripts
                    ),
                    cancellationToken
                );
                if (!consolidationResult.Success)
                {
                    return new(
                        false,
                        consolidationResult.ErrorCode
                    );
                }
                consolidatedScripts = consolidationResult.Result.ConsolidatedScripts;

                // Load all Assembly Libraries into Evaluator
                var scriptAssemblies = await _mediator.Send(
                    new QueryForScriptAssemblyList(),
                    cancellationToken
                );
                var assemblyNames = new List<string>();

                foreach (var assembly in scriptAssemblies)
                {
                    _builder.ReferenceAssembly(
                        assembly
                    );
                    if (string.IsNullOrEmpty(
                        assembly.FullName
                    ))
                    {
                        continue;
                    }
                    assemblyNames.Add(
                        assembly.FullName
                    );
                }

                // Create Assembly, returns reference to generated File
                var generatedFileFullName = await _builder.Compile(
                    consolidatedScripts
                );

                // Create Hash For Content
                var scriptHashResult = await _mediator.Send(
                    new CreateHashFromContentCommand(
                        consolidatedScripts
                    ),
                    cancellationToken
                );
                var scriptHash = scriptHashResult.Result;

                // Log some information
                _logger.LogInformation(
                    "Script Assembly References Added: \n\r\t {ScriptAssemblyNames}",
                    assemblyNames
                );
                _logger.LogInformation(
                    "Script Source Hash: {ScriptHash}",
                    scriptHash
                );

                return new(
                    scriptHash
                );
            }
            catch (Exception ex)
            {
                var consolidatedScriptsSavePath = Path.Combine(
                    _serverInfo.FileSystemTempPath,
                    "ConsolidatedServerScripts.csx"
                );
                await WriteAllTextToFile(
                    consolidatedScriptsSavePath,
                    consolidatedScripts,
                    cancellationToken
                );

                _logger.LogError(
                    ex,
                    "Exception thrown while compiling. \n\r\tCheckout {ConsolidatedScriptsSavePath} for the Generated ConsolidatedScripts.",
                    consolidatedScriptsSavePath
                );

                return new(
                    false,
                    "csharp_failed_to_compile_server_scripts"
                );
            }
        }

        private Task WriteAllTextToFile(
            string fileFullName,
            string fileContents,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new WriteAllTextToFile(
                fileFullName,
                fileContents
            ),
            cancellationToken
        );
    }
}
