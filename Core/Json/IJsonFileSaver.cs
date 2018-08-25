using System.Threading.Tasks;

namespace EventHorizon.Game.Server.Zone.Core.Json
{
    public interface IJsonFileSaver
    {
        Task SaveToFile(string directory, string fileName, object value);
    }
}