namespace EventHorizon.Zone.Core.Map.Tests.Model
{
    using EventHorizon.Zone.Core.Map.Model;
    using FluentAssertions;
    using Xunit;

    public class ZoneMapDetailsTests
    {
        [Fact]
        public void ShouldContainExpectedValuesWhenCreated()
        {
            // Given
            var dimensions = 121;
            var tileDimensions = 231;
            var heightMapUrl = "height-map-url";
            var light = "light";
            var width = 1;
            var height = 12;
            var subdivisions = 123;
            var minHeight = 1231;
            var maxHeight = 12312;
            var updatable = true;
            var isPickable = false;
            var material = default(MapMeshMaterial);

            // When
            var actual = new ZoneMapDetails
            {
                Dimensions = dimensions,
                TileDimensions = tileDimensions,
                Mesh = new ZoneMapMesh
                {
                    HeightMapUrl = heightMapUrl,
                    Light = light,
                    Width = width,
                    Height = height,
                    Subdivisions = subdivisions,
                    MinHeight = minHeight,
                    MaxHeight = maxHeight,
                    Updatable = updatable,
                    IsPickable = isPickable,
                    Material = material,
                },
            };

            // Then
            actual.Dimensions.Should().Be(dimensions);
            actual.TileDimensions.Should().Be(tileDimensions);
            actual.Mesh.HeightMapUrl
                .Should().Be(heightMapUrl);
            actual.Mesh.Light
                .Should().Be(light);
            actual.Mesh.Width
                .Should().Be(width);
            actual.Mesh.Height
                .Should().Be(height);
            actual.Mesh.Subdivisions
                .Should().Be(subdivisions);
            actual.Mesh.MinHeight
                .Should().Be(minHeight);
            actual.Mesh.MaxHeight
                .Should().Be(maxHeight);
            actual.Mesh.Updatable
                .Should().Be(updatable);
            actual.Mesh.IsPickable
                .Should().Be(isPickable);
            actual.Mesh.Material
                .Should().Be(material);
        }
    }
}
