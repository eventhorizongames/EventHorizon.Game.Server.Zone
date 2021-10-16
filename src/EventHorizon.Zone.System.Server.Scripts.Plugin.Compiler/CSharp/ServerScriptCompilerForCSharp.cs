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
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Linq;
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
                        success: false,
                        consolidationResult.ErrorCode,
                        scriptErrorDetailsList: new List<GeneratedServerScriptErrorDetailsModel>()
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
                    "csharp_failed_to_compile_server_scripts",
                    GenerateErrorDetails(
                        consolidatedScripts,
                        ex.Message
                    )
                );
            }
        }

        private static List<GeneratedServerScriptErrorDetailsModel> GenerateErrorDetails(
            string consolidatedScripts,
            string errorMessage
        )
        {
            var result = new List<GeneratedServerScriptErrorDetailsModel>();
            // Parse Error Message
            var errorMessageList = errorMessage.Split(
                "\r\n",
                StringSplitOptions.RemoveEmptyEntries
            );

            foreach (var message in errorMessageList)
            {
                var lineAndColumn = message.Split(":")
                    .FirstOrDefault()
                    ?.Replace("(", string.Empty)
                    .Replace(")", string.Empty)
                    .Split(",").ToList() ?? new List<string>();
                if (lineAndColumn.Count != 2)
                {
                    continue;
                }

                var line = int.Parse(lineAndColumn[0]);
                var column = int.Parse(lineAndColumn[1]);
                var (ScriptId, ErrorLineContent) = GetScriptIdAndLineContent(
                    consolidatedScripts,
                    line
                );
                var errorMessageDetails = new GeneratedServerScriptErrorDetailsModel
                {
                    ScriptId = ScriptId,
                    Message = message.Replace(
                        $"({line},{column}): error",
                        string.Empty
                    ).Replace(
                        $"({line}, {column}): error",
                        string.Empty
                    ).Trim(),
                    ErrorLineContent = ErrorLineContent,
                    Column = column,
                };

                result.Add(
                    errorMessageDetails
                );
            }

            return result;
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

        public static (string ScriptId, string LineContent) GetScriptIdAndLineContent(
            string consolidatedScripts,
            int line
        )
        {
            var lines = consolidatedScripts.Split(
                Environment.NewLine
            );
            var lineContent = lines.Skip(line - 1).FirstOrDefault() ?? string.Empty;

            var fileLines = new List<string>
            {
                lineContent,
            };

            while (!FindTopFileSection(
                lines,
                fileLines,
                line - 1
            ))
            {
            }
            var scriptIdLine = fileLines
                .FirstOrDefault() ?? string.Empty;

            var scriptId = scriptIdLine.Replace(
                "// Script Id: ",
                string.Empty
            );

            return (
                scriptId,
                lineContent
            );
        }

        public static bool FindTopFileSection(
            string[] lines,
            List<string> fileContent,
            int lineToCheck
        )
        {
            if (lineToCheck < 0)
            {
                // Line to Check is out of bounds"
                return true;
            }

            var line = lines.Skip(lineToCheck - 1).FirstOrDefault() ?? string.Empty;
            if (line.StartsWith("// === FILE_START ==="))
            {
                return true;
            }

            fileContent.Insert(
                0,
                line
            );

            return FindTopFileSection(
                lines,
                fileContent,
                lineToCheck - 1
            );
        }
    }
}
