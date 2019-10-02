using System.IO;
using System.Text;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Json;
using Newtonsoft.Json;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Core.Json
{
    public class JsonFileSaver : IJsonFileSaver
    {
        public async Task SaveToFile(
            string directory,
            string fileName,
            object value
        )
        {
            await WriteToFile(
                directory,
                fileName,
                JsonConvert.SerializeObject(
                    value
                )
            );
        }
        private async Task WriteToFile(
            string directory,
            string fileName,
            string jsonString
        )
        {
            Directory.CreateDirectory(
                directory
            );
            using (var file = File.Create(
                IOPath.Combine(
                    directory,
                    fileName
                )
            ))
            {
                await file.WriteAsync(
                    Encoding.UTF8.GetBytes(
                        jsonString
                    )
                );
            }
        }
    }
}