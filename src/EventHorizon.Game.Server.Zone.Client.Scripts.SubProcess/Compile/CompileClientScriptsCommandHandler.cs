namespace EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess.Compile
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using MediatR;
    using EventHorizon.Zone.System.Client.Scripts.Model.Generated;
    using EventHorizon.Zone.Core.Model.Json;

    public class CompileClientScriptsCommandHandler
        : IRequestHandler<CompileClientScriptsCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly ClientScriptCompiler _clientScriptCompiler;
        private readonly IJsonFileSaver _jsonFileSaver;

        public CompileClientScriptsCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            ClientScriptCompiler clientScriptCompiler,
            IJsonFileSaver jsonFileSaver
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _clientScriptCompiler = clientScriptCompiler;
            _jsonFileSaver = jsonFileSaver;
        }

        public async Task<StandardCommandResult> Handle(
            CompileClientScriptsCommand request,
            CancellationToken cancellationToken
        )
        {
            // Load all scripts from File System
            var clientScriptList = new List<ClientScript>();
            await _mediator.Send(
                new ProcessFilesRecursivelyFromDirectory(
                    _serverInfo.ClientScriptsPath,
                    async (
                        fileInfo,
                        arguments
                    ) =>
                    {
                        if (fileInfo.Extension != ".csx")
                        {
                            return;
                        }

                        var rootPath = arguments["RootPath"] as string;
                        // Create ClientScript AND Add to Repository
                        clientScriptList.Add(
                            ClientScript.Create(
                                ClientScriptType.CSharp,
                                rootPath.MakePathRelative(
                                    fileInfo.DirectoryName
                                ),
                                fileInfo.Name,
                                await _mediator.Send(
                                    new ReadAllTextFromFile(
                                        fileInfo.FullName
                                    )
                                )
                            )
                        );
                    },
                    new Dictionary<string, object>
                    {
                        ["RootPath"] = _serverInfo.ClientScriptsPath,
                    }
                ),
                cancellationToken
            );
            // Compile Scripts & Create a DLL
            var result = await _clientScriptCompiler.Compile(
                clientScriptList
            );

            var generatedScriptResult = new GeneratedClientScriptsResultModel
            {
                Success = result.Success,
                ErrorCode = result.ErrorCode,
                Hash = result.Hash,
                ScriptAssembly = result.ScriptAssembly,
            };

            await _jsonFileSaver.SaveToFile(
                _serverInfo.GeneratedPath,
                GeneratedClientScriptsResultModel.GENERATED_FILE_NAME,
                generatedScriptResult
            );

            return new();
        }
    }
}
