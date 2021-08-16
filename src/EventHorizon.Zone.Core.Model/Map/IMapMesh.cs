namespace EventHorizon.Zone.Core.Model.Map
{
    public interface IMapMesh
    {
        string HeightMapUrl { get; }
        int Width { get; }
        int Height { get; }
        int Subdivisions { get; }
        int MinHeight { get; }
        int MaxHeight { get; }
        bool Updatable { get; }
        bool IsPickable { get; }
    }
}
