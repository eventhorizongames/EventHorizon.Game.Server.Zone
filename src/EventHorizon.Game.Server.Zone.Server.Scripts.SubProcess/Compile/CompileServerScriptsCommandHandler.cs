namespace EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess.Compile
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Model.Generated;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Server.Scripts.Shared.Model;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public partial class CompileServerScriptsCommandHandler
        : IRequestHandler<CompileServerScriptsCommand, StandardCommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTimeService;
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileSaver _jsonFileSaver;
        private readonly ServerScriptCompiler _serverScriptCompiler;

        public CompileServerScriptsCommandHandler(
            ILogger<CompileServerScriptsCommandHandler> logger,
            IMediator mediator,
            IDateTimeService dateTimeService,
            ServerInfo serverInfo,
            IJsonFileSaver jsonFileSaver,
            ServerScriptCompiler serverScriptCompiler
        )
        {
            _logger = logger;
            _mediator = mediator;
            _dateTimeService = dateTimeService;
            _serverInfo = serverInfo;
            _jsonFileSaver = jsonFileSaver;
            _serverScriptCompiler = serverScriptCompiler;
        }

        public async Task<StandardCommandResult> Handle(
            CompileServerScriptsCommand request,
            CancellationToken cancellationToken
        )
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation(
                "Starting Server Scripts Compile. {StartDateTime}",
                _dateTimeService.Now.ToString()
            );
            // Load all scripts from File System
            var serverScriptList = new List<ServerScriptDetails>();
            await _mediator.Send(
                new ProcessFilesRecursivelyFromDirectory(
                    _serverInfo.ServerScriptsPath,
                    async (
                        fileInfo,
                        arguments
                    ) =>
                    {
                        if (!fileInfo.FullName.EndsWith(
                            ".csx"
                        ))
                        {
                            return;
                        }

                        var rootPath = arguments["RootPath"] as string;
                        serverScriptList.Add(
                            new ServerScriptDetails(
                                fileName: fileInfo.Name.Replace(".csx", string.Empty),
                                path: rootPath.MakePathRelative(
                                    fileInfo.DirectoryName
                                ),
                                scriptString: await _mediator.Send(
                                    new ReadAllTextFromFile(
                                        fileInfo.FullName
                                    )
                                )
                            )
                        );
                    },
                    new Dictionary<string, object>
                    {
                        ["RootPath"] = _serverInfo.ServerScriptsPath,
                    }
                ),
                cancellationToken
            );

            _logger.LogInformation(
                "Loaded {ScriptCount} Server Scripts.",
                serverScriptList.Count
            );

            // Compile Scripts & Create a DLL
            var result = await _serverScriptCompiler.Compile(
                serverScriptList,
                cancellationToken
            );

            var generatedScriptResult = new GeneratedServerScriptsResultModel
            {
                Success = result.Success,
                ErrorCode = result.ErrorCode,
                Hash = result.Hash,
            };

            await _jsonFileSaver.SaveToFile(
                _serverInfo.GeneratedPath,
                GeneratedServerScriptsResultModel.SCRIPTS_RESULT_FILE_NAME,
                generatedScriptResult
            );

            _logger.LogInformation(
                "Finished Server Scripts Compile. {FinishedDateTime} | {ElapsedTimespan}",
                _dateTimeService.Now.ToString(),
                stopwatch.Elapsed
            );

            return new();
        }
    }
}
