namespace EventHorizon.Zone.Core.Json;

using System.Text.Json;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Json;

public class TextJsonFileLoader : IJsonFileLoader
{
    private readonly FileResolver _fileResolver;

    public TextJsonFileLoader(FileResolver fileResolver)
    {
        _fileResolver = fileResolver;
    }

    public Task<T?> GetFile<T>(string fileFullName)
    {
        if (!_fileResolver.DoesFileExist(fileFullName))
        {
            return default(T).FromResult();
        }
        return JsonSerializer
            .Deserialize<T>(
                _fileResolver.GetFileText(fileFullName),
                IJsonFileSaver.DEFAULT_JSON_OPTIONS
            )
            .FromResult();
    }
}
