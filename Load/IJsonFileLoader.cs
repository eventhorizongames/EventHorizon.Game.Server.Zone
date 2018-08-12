using System.Threading.Tasks;

namespace EventHorizon.Game.Server.Zone.Load
{
    public interface IJsonFileLoader
    {
          Task<T> GetFile<T>(string fileName);
    }
}