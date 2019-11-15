using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Json;
using Newtonsoft.Json;

namespace EventHorizon.Zone.Core.Json
{
    public class NewtonsoftJsonFileLoader : IJsonFileLoader
    {
        readonly FileResolver _fileResolver;

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
                return Task.FromResult(
                    default(T)
                );
            }
            return Task.FromResult(
                JsonConvert.DeserializeObject<T>(
                    _fileResolver.GetFileText(
                        fileFullName
                    )
                )
            );
        }
    }
}