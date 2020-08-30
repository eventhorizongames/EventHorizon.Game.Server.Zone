namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Builders
{
    using global::System.IO;
    using global::System.Reflection;
    using global::System.Threading.Tasks;
    using CSScriptLib;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using MediatR;

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
                "Scripts_temp.dll"
            );

            _evaluator.CompileAssemblyFromCode(
                assemblyAsString,
                tempFile
            );

            return SaveTempFile(
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

        private async Task<string> SaveTempFile(
            string tempFile
        )
        {
            var path = Path.Combine(
                _serverInfo.AppDataPath,
                "_generated"
            );
            var exists = await _mediator.Send(
                new DoesDirectoryExist(
                    path
                )
            );
            if (!exists)
            {
                await _mediator.Send(
                    new CreateDirectory(
                        path
                    )
                );
            }
            var fileFullName = Path.Combine(
                path,
                "Scripts.dll"
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