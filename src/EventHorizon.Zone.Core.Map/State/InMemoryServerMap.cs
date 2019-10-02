using EventHorizon.Zone.Core.Model.Map;

namespace EventHorizon.Zone.Core.Map.State
{
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
    public class InMemoryServerMap : IServerMap
    {
        private IMapGraph _map;
        private IMapDetails _mapDetails;
        private IMapMesh _mapMesh;

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
            this._map = map;
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
}