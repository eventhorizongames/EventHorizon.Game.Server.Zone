namespace EventHorizon.Zone.Core.Json
{
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Json;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class NewtonsoftJsonFileLoader : IJsonFileLoader
    {
        private readonly FileResolver _fileResolver;

        public NewtonsoftJsonFileLoader(
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
            return JsonConvert.DeserializeObject<T>(
                _fileResolver.GetFileText(
                    fileFullName
                )
            ).FromResult();
        }
    }
}