using System.Numerics;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Math;

namespace EventHorizon.Game.Server.Zone.Entity.State
{
    public interface IEntitySearchTree
    {
        void UpdateDimensions(Vector3 dimensions);
        void Update(SearchEntity searchEntity);
    }
}