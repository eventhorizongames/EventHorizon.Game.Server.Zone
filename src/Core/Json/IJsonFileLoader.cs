using System.Threading.Tasks;

namespace EventHorizon.Game.Server.Zone.Core.Json
{
    public interface IJsonFileLoader
    {
          Task<T> GetFile<T>(string fileName);
    }
}