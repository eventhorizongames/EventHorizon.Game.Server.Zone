namespace EventHorizon.Zone.Core.Model.Map.Client;

using EventHorizon.Zone.Core.Model.Client;

public struct CoreMapLoadedClientData : IClientActionData
{
    public IMapGraph Map { get; }
    public IMapMesh MapMesh { get; }

    public CoreMapLoadedClientData(
        IMapGraph map,
        IMapMesh mapMesh
    )
    {
        Map = map;
        MapMesh = mapMesh;
    }
}
