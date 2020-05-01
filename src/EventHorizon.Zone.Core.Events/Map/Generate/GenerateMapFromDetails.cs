namespace EventHorizon.Zone.Core.Events.Map.Generate
{
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public struct GenerateMapFromDetails : IRequest<IMapGraph>
    {
        public IMapDetails MapDetails { get; }

        public GenerateMapFromDetails(
            IMapDetails mapDetails
        )
        {
            MapDetails = mapDetails;
        }
    }
}
