using System.Threading.Tasks;

namespace EventHorizon.Zone.Core.Model.Json
{
    public interface IJsonFileLoader
    {
        Task<T> GetFile<T>(string fileName);
    }
}
