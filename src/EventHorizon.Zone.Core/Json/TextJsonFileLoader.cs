namespace EventHorizon.Zone.Core.Json
{
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Json;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class TextJsonFileLoader : IJsonFileLoader
    {
        private static JsonSerializerOptions JSON_OPTIONS = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly FileResolver _fileResolver;

        public TextJsonFileLoader(
            FileResolver fileResolver
        )
        {
            _fileResolver = fileResolver;
        }

        public Task<T> GetFile<T>(
            string fileFullName
        )
        {
            if (!_fileResolver.DoesFileExist(
                fileFullName
            ))
            {
                return default(T).FromResult();
            }
            return JsonSerializer.Deserialize<T>(
                _fileResolver.GetFileText(
                    fileFullName
                ),
                JSON_OPTIONS
            ).FromResult();
        }
    }
}