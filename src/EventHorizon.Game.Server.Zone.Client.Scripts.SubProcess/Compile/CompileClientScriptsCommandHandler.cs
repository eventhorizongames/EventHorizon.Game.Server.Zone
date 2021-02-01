namespace EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess.Compile
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
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Model.Generated;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class CompileClientScriptsCommandHandler
        : IRequestHandler<CompileClientScriptsCommand, StandardCommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTimeService;
        private readonly ServerInfo _serverInfo;
        private readonly ClientScriptCompiler _clientScriptCompiler;
        private readonly IJsonFileSaver _jsonFileSaver;

        public CompileClientScriptsCommandHandler(
            ILogger<CompileClientScriptsCommandHandler> logger,
            IMediator mediator,
            IDateTimeService dateTimeService,
            ServerInfo serverInfo,
            ClientScriptCompiler clientScriptCompiler,
            IJsonFileSaver jsonFileSaver
        )
        {
            _logger = logger;
            _mediator = mediator;
            _dateTimeService = dateTimeService;
            _serverInfo = serverInfo;
            _clientScriptCompiler = clientScriptCompiler;
            _jsonFileSaver = jsonFileSaver;
        }

        public async Task<StandardCommandResult> Handle(
            CompileClientScriptsCommand request,
            CancellationToken cancellationToken
        )
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation(
                "Starting Client Scripts Compile. {StartDateTime}",
                _dateTimeService.Now.ToString()
            );
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

            _logger.LogInformation(
                "Loaded {ScriptCount} Client Scripts.",
                clientScriptList.Count
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

            _logger.LogInformation(
                "Finished Client Scripts Compile. {FinishedDateTime} | {ElapsedTimespan}",
                _dateTimeService.Now.ToString(),
                stopwatch.Elapsed
            );

            return new();
        }
    }
}
