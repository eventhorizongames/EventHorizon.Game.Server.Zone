using System.IO;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Json;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Core.Json
{
    public class JsonFileLoader : IJsonFileLoader
    {
        public async Task<T> GetFile<T>(
            string fileName
        )
        {
            if (AssetsFileExists(
                fileName
            ))
            {
                using (var settingsFile = File.OpenText(
                    fileName
                ))
                {
                    return JsonConvert.DeserializeObject<T>(
                        await settingsFile.ReadToEndAsync()
                    );
                }
            }
            return default(T);
        }
        private bool AssetsFileExists(
            string fileName
        )
        {
            return File.Exists(
                fileName
            );
        }
    }
}