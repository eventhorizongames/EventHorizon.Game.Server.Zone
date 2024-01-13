namespace EventHorizon.Zone.Core.Model.Json;

using System.Threading.Tasks;

public interface IJsonFileSaver
{
    Task SaveToFile(string directory, string fileName, object value);
}
