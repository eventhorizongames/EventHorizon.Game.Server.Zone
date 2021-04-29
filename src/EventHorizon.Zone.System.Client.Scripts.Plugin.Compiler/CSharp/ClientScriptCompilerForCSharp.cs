namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.CSharp
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Assemblies;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Consolidate;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Create;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ClientScriptCompilerForCSharp
        : ClientScriptCompiler
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly AssemblyBuilder _builder;

        public ClientScriptCompilerForCSharp(
            ILogger<ClientScriptCompilerForCSharp> logger,
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
            IEnumerable<ClientScript> scripts,
            CancellationToken cancellationToken
        )
        {
            var consolidatedScripts = string.Empty;
            try
            {
                var consolidationResult = await _mediator.Send(
                    new ConsolidateClientScriptsCommand(
                        scripts
                    ),
                    cancellationToken
                );
                if (!consolidationResult)
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

                // Create EncodedScriptFile
                var encodedFileContent = Convert.ToBase64String(
                    await ReadFileAsBytes(
                        generatedFileFullName
                    )
                );

                // Log some information
                _logger.LogInformation(
                    "Client Script Assembly References Added: \n\r\t {ScriptAssemblyNames}",
                    assemblyNames
                );
                _logger.LogInformation(
                    "Client Script Source Hash: {ScriptHash}",
                    scriptHash
                );

                return new(
                    scriptHash,
                    encodedFileContent
                );
            }
            catch (Exception ex)
            {
                var consolidatedScriptsSavePath = Path.Combine(
                    _serverInfo.FileSystemTempPath,
                    "ConsolidatedScripts.csx"
                );
                await WriteAllTextToFile(
                    consolidatedScriptsSavePath,
                    consolidatedScripts
                );

                _logger.LogError(
                    ex,
                    "Exception thrown while compiling. \n\r\tCheckout {ConsolidatedScriptsSavePath} for the Generated ConsolidatedScripts.",
                    consolidatedScriptsSavePath
                );

                return new(
                    false,
                    "csharp_failed_to_compile"
                );
            }
        }

        private Task<byte[]> ReadFileAsBytes(
            string fileFullName
        ) => _mediator.Send(
            new ReadAllTextAsBytesFromFile(
                fileFullName
            )
        );

        private Task WriteAllTextToFile(
            string fileFullName,
            string fileContents
        ) => _mediator.Send(
            new WriteAllTextToFile(
                fileFullName,
                fileContents
            )
        );
    }
}
