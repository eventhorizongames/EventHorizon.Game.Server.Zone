namespace EventHorizon.Zone.Core.Map.State;

using System.Numerics;

using EventHorizon.Zone.Core.Map.Model;
using EventHorizon.Zone.Core.Model.Map;

public class InMemoryServerMap
    : IServerMap
{
    private IMapGraph _map = new MapGraph(Vector3.Zero, Vector3.One, false);
    private IMapDetails _mapDetails = new ZoneMapDetails();
    private IMapMesh _mapMesh = new ZoneMapMesh();

    public IMapGraph Map()
    {
        return _map;
    }

    public IMapDetails MapDetails()
    {
        return _mapDetails;
    }

    public IMapMesh MapMesh()
    {
        return _mapMesh;
    }

    public void SetMap(
        IMapGraph map
    )
    {
        _map = map;
    }

    public void SetMapDetails(
        IMapDetails mapDetails
    )
    {
        _mapDetails = mapDetails;
    }

    public void SetMapMesh(
        IMapMesh mapMesh
    )
    {
        _mapMesh = mapMesh;
    }
}
