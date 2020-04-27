namespace EventHorizon.Zone.Core.Json
{
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Json;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class TextJsonFileSaver : IJsonFileSaver
    {
        readonly DirectoryResolver _directoryResolver;
        readonly FileResolver _fileResolver;

        public TextJsonFileSaver(
            DirectoryResolver directoryResolver,
            FileResolver fileResolver
        )
        {
            _directoryResolver = directoryResolver;
            _fileResolver = fileResolver;
        }

        public Task SaveToFile(
            string directoryFullName,
            string fileFullName,
            object value
        )
        {
            if (!_directoryResolver.DoesDirectoryExist(
                directoryFullName
            ))
            {
                _directoryResolver.CreateDirectory(
                    directoryFullName
                );
            }
            _fileResolver.WriteAllText(
                Path.Combine(
                    directoryFullName,
                    fileFullName
                ),
                JsonSerializer.Serialize(
                    value
                )
            );
            return Task.CompletedTask;
        }
    }
}