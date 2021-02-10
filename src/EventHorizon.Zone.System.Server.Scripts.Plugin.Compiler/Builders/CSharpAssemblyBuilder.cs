namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Builders
{
    using global::System.IO;
    using global::System.Reflection;
    using global::System.Threading.Tasks;
    using CSScriptLib;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Api;
    using MediatR;
    using EventHorizon.Zone.System.Server.Scripts.Model.Generated;

    public class CSharpAssemblyBuilder
        : AssemblyBuilder
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly IEvaluator _evaluator;

        public CSharpAssemblyBuilder(
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _evaluator = CSScript.Evaluator;
        }

        public Task<string> Compile(
            string assemblyAsString
        )
        {
            var tempFile = Path.Combine(
                _serverInfo.FileSystemTempPath,
                "Server_Scripts_temp.dll"
            );

            _evaluator.CompileAssemblyFromCode(
                assemblyAsString,
                tempFile
            );

            return SaveGeneratedFile(
                tempFile
            );
        }

        public void ReferenceAssembly(
            Assembly assembly
        )
        {
            _evaluator.ReferenceAssembly(
                assembly
            );
        }

        private async Task<string> SaveGeneratedFile(
            string tempFile
        )
        {
            var exists = await _mediator.Send(
                new DoesDirectoryExist(
                    _serverInfo.GeneratedPath
                )
            );
            if (!exists)
            {
                await _mediator.Send(
                    new CreateDirectory(
                        _serverInfo.GeneratedPath
                    )
                );
            }
            var fileFullName = Path.Combine(
                _serverInfo.GeneratedPath,
                GeneratedServerScriptsResultModel.SCRIPTS_ASSEMBLY_FILE_NAME
            );

            var tempFileContent = File.ReadAllBytes(
                tempFile
            );

            await _mediator.Send(
                new WriteAllBytesToFile(
                    fileFullName,
                    tempFileContent
                )
            );

            return fileFullName;
        }
    }
}