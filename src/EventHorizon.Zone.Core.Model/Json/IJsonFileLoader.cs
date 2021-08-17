namespace EventHorizon.Zone.Core.Model.Json
{
    using System.Threading.Tasks;

    public interface IJsonFileLoader
    {
        Task<T?> GetFile<T>(string fileName);
    }
}
