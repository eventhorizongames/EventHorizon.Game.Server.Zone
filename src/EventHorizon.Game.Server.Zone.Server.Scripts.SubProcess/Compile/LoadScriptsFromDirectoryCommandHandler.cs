namespace EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess.Compile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;

    using MediatR;

    public class LoadScriptsFromDirectoryCommandHandler
        : IRequestHandler<LoadScriptsFromDirectoryCommand, CommandResult<IEnumerable<ServerScriptDetails>>>
    {
        private readonly IMediator _mediator;

        public LoadScriptsFromDirectoryCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<CommandResult<IEnumerable<ServerScriptDetails>>> Handle(
            LoadScriptsFromDirectoryCommand request,
            CancellationToken cancellationToken
        )
        {
            var scriptList = new List<ServerScriptDetails>();

            await _mediator.Send(
                new ProcessFilesRecursivelyFromDirectory(
                    request.DirectoryFullName,
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
                        scriptList.Add(
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
                    request.Arguments
                ),
                cancellationToken
            );

            return scriptList;
        }
    }
}
