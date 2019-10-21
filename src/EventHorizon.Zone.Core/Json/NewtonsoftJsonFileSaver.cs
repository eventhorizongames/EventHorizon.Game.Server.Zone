using System.IO;
using System.Text;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Json;
using Newtonsoft.Json;

namespace EventHorizon.Zone.Core.Json
{
    public class NewtonsoftJsonFileSaver : IJsonFileSaver
    {
        public Task SaveToFile(
            string directory,
            string fileName,
            object value
        )
        {
            WriteToFile(
                directory,
                fileName,
                JsonConvert.SerializeObject(
                    value
                )
            );
            return Task.CompletedTask;
        }
        private void WriteToFile(
            string directory,
            string fileName,
            string jsonString
        )
        {
            Directory.CreateDirectory(
                directory
            );
            File.WriteAllText(
                Path.Combine(
                    directory,
                    fileName
                ),
                jsonString
            );
        }
    }
}