namespace EventHorizon.Zone.Core.FileService
{
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using EventHorizon.Zone.Core.Model.FileService;

    using MediatR;

    public class WriteResourceToFileHandler
        : IRequestHandler<WriteResourceToFile, StandardCommandResult>
    {
        private readonly FileResolver _fileResolver;
        private readonly DirectoryResolver _directoryResolver;

        public WriteResourceToFileHandler(
            FileResolver fileResolver,
            DirectoryResolver directoryResolver
        )
        {
            _fileResolver = fileResolver;
            _directoryResolver = directoryResolver;
        }

        public Task<StandardCommandResult> Handle(
            WriteResourceToFile request,
            CancellationToken cancellationToken
        )
        {
            if (_fileResolver.DoesFileExist(
                request.SaveFileFullName
            ))
            {
                return new StandardCommandResult(
                    "file_already_exists"
                ).FromResult();
            }

            // Get File Resource
            var fileContent = LoadFileFromResources(
                request.ExecutingAssembly,
                request.ResourceRoot,
                request.ResourcePath,
                request.ResourceFile
            );
            if (string.IsNullOrEmpty(
                fileContent
            ))
            {
                return new StandardCommandResult(
                    "resource_not_found"
                ).FromResult();
            }

            var directorName = Path.GetDirectoryName(
                request.SaveFileFullName
            );
            if (string.IsNullOrEmpty(
                directorName
            ))
            {
                return new StandardCommandResult(
                    "DIRECTORY_NAME_EMPTY"
                ).FromResult();
            }

            _directoryResolver.CreateDirectory(
                directorName
            );

            _fileResolver.WriteAllText(
                request.SaveFileFullName,
                fileContent
            );

            return new StandardCommandResult()
                .FromResult();
        }

        private static string LoadFileFromResources(
            Assembly executingAssembly,
            string resourceRoot,
            string resourcePath,
            string resourceFile
        )
        {
            using var stream = executingAssembly.GetManifestResourceStream(
                $"{resourceRoot}.{resourcePath}.{resourceFile}"
            );
            if (stream == null)
            {
                return string.Empty;
            }
            using var reader = new StreamReader(
                stream
            );

            return reader.ReadToEnd();
        }
    }
}
