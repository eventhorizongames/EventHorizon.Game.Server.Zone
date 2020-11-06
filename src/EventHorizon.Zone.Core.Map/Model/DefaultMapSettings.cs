namespace EventHorizon.Zone.Core.Map.Model
{
    using System;

    public static class DefaultMapSettings
    {
        public static ZoneMapDetails DEFAULT_MAP = new ZoneMapDetails
        {
            Dimensions = 16,
            TileDimensions = 1,
            Mesh = new ZoneMapMesh
            {
                HeightMapUrl = string.Empty,
                Light = string.Empty,
                Width = 32,
                Height = 32,
                Subdivisions = 200,
                MinHeight = 0,
                MaxHeight = 12,
                Material = default,
            }
        };
    }
}
