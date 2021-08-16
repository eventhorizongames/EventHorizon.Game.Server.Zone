namespace EventHorizon.Zone.Core.Json
{
    using System.IO;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Model.DirectoryService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Json;

    using Newtonsoft.Json;

    public class NewtonsoftJsonFileSaver : IJsonFileSaver
    {
        readonly DirectoryResolver _directoryResolver;
        readonly FileResolver _fileResolver;

        public NewtonsoftJsonFileSaver(
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
                JsonConvert.SerializeObject(
                    value
                )
            );
            return Task.CompletedTask;
        }
    }
}
