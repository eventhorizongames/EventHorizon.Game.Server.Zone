using System.Threading.Tasks;

namespace EventHorizon.Zone.Core.Model.Json
{
    public interface IJsonFileSaver
    {
        Task SaveToFile(string directory, string fileName, object value);
    }
}