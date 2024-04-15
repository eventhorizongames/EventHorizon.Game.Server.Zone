namespace EventHorizon.Zone.Core.Model.Json;

using System.Text.Json;
using System.Threading.Tasks;

public interface IJsonFileSaver
{
    public static readonly JsonSerializerOptions DEFAULT_JSON_OPTIONS =
        new() { PropertyNameCaseInsensitive = true, WriteIndented = true, };

    Task SaveToFile(string directory, string fileName, object value);
}
