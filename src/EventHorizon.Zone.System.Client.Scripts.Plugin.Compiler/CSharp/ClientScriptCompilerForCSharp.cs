namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.CSharp
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using MediatR;
    using EventHorizon.Zone.Core.Events.FileService;
    using Microsoft.Extensions.Logging;
    using global::System.Reflection;
    using global::System.Linq;
    using global::System.Security.Cryptography;
    using global::System.Text;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;

    public class ClientScriptCompilerForCSharp
        : ClientScriptCompiler
    {
        private static Assembly[] GetScriptAssemblies()
        {
            // TODO: Look at using CQRS
            var assemblies = Directory.GetFiles(
                AppDomain.CurrentDomain.BaseDirectory,
                "EventHorizon.*.dll"
            ).Select(
                x => Assembly.Load(
                    AssemblyName.GetAssemblyName(x)
                )
            );
            return assemblies.ToArray();
        }
        private readonly static string AssemblyScriptTemplate = @"                        
[[USING_SECTION]]

[[SCRIPT_CLASSES]]
";

        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly AssemblyBuilder _builder;
        private readonly ClientScriptsConsolidator _clientScriptsConsolidator;

        public ClientScriptCompilerForCSharp(
            ILogger<ClientScriptCompilerForCSharp> logger,
            IMediator mediator,
            AssemblyBuilder builder,
            ClientScriptsConsolidator clientScriptsConsolidator
        )
        {
            _logger = logger;
            _mediator = mediator;
            _builder = builder;
            _clientScriptsConsolidator = clientScriptsConsolidator;
        }

        public async Task<CompiledScriptResult> Compile(
            IEnumerable<ClientScript> scripts
        )
        {
            try
            {
                var usingList = new List<string>();

                var scriptClasses = _clientScriptsConsolidator.IntoSingleTemplatedString(
                    scripts,
                    ref usingList
                );
                var usingSection = string.Join(
                    Environment.NewLine,
                    usingList
                );

                var consolidatedClasses = AssemblyScriptTemplate.Replace(
                    "[[USING_SECTION]]",
                    usingSection
                ).Replace(
                    "[[SCRIPT_CLASSES]]",
                    scriptClasses
                );

                // Load all Assembly Libraries into Evaluator
                var scriptAssemblies = GetScriptAssemblies();
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
                    consolidatedClasses
                );

                // Create Hash For Content
                var scriptHash = CreateHashFromContent(
                    consolidatedClasses
                );

                // Create EncodedScriptFile
                var encodedFileContent = Convert.ToBase64String(
                    await ReadFileAsBytes(
                        generatedFileFullName
                    )
                );

                // Log some information
                _logger.LogInformation(
                    "Script Assembly References Added: \n\r\t {ScriptAssemblyNames}",
                    assemblyNames
                );
                _logger.LogInformation(
                    "Script Source Hash: {ScriptHash}",
                    scriptHash
                );

                return new CompiledScriptResult(
                    scriptHash,
                    encodedFileContent
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Exception thrown while compiling"
                );
                // TODO: Look a more informative error result
                return new CompiledScriptResult(
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

        private string CreateHashFromContent(
            string consolidatedClasses
        )
        {
            using var sha256Hash = SHA256.Create();
            var scriptHashBytes = sha256Hash.ComputeHash(
                consolidatedClasses.ToBytes()
            );

            // Convert byte array to a string   
            var hashBuilder = new StringBuilder();
            for (int i = 0; i < scriptHashBytes.Length; i++)
            {
                hashBuilder.Append(
                    scriptHashBytes[i].ToString("x2")
                );
            }
            return hashBuilder.ToString();
        }
    }
}
