namespace EventHorizon.Zone.Core.FileService
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;

    using MediatR;

    public class ProcessFilesRecursivelyFromDirectoryHandler : IRequestHandler<ProcessFilesRecursivelyFromDirectory>
    {
        readonly IMediator _mediator;

        public ProcessFilesRecursivelyFromDirectoryHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(
            ProcessFilesRecursivelyFromDirectory request,
            CancellationToken cancellationToken
        )
        {
            await LoadFromDirectoryInfo(
                request.FromDirectory,
                request.OnProcessFile,
                request.Arguments
            );
            return Unit.Value;
        }

        private async Task LoadFromDirectoryInfo(
            string directoryFullName,
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile,
            IDictionary<string, object> arguments
        )
        {
            // Load Scripts from Sub-Directories
            foreach (var subDirectoryInfo in await _mediator.Send(
                new GetListOfDirectoriesFromDirectory(
                    directoryFullName
                )
            ))
            {
                // Load Files From Directories
                await LoadFromDirectoryInfo(
                    subDirectoryInfo.FullName,
                    onProcessFile,
                    arguments
                );
            }
            // Load script files into Repository
            await LoadFileIntoRepository(
                directoryFullName,
                onProcessFile,
                arguments
            );
        }


        private async Task LoadFileIntoRepository(
            string directoryFullName,
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile,
            IDictionary<string, object> arguments
        )
        {
            foreach (var fileInfo in await _mediator.Send(
                new GetListOfFilesFromDirectory(
                    directoryFullName
                )
            ))
            {
                await onProcessFile(
                    fileInfo,
                    arguments
                );
            }
        }
    }
}
