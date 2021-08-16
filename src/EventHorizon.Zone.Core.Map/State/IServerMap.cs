namespace EventHorizon.Zone.Core.Map.State
{
    using EventHorizon.Zone.Core.Model.Map;

    public interface IServerMap
    {
        IMapGraph Map();
        void SetMap(
            IMapGraph map
        );
        IMapDetails MapDetails();
        void SetMapDetails(
            IMapDetails mapDetails
        );
        IMapMesh MapMesh();
        void SetMapMesh(
            IMapMesh mapMesh
        );
    }
}
