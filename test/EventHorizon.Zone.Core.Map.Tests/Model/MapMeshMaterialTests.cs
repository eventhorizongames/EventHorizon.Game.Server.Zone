namespace EventHorizon.Zone.Core.Map.Tests.Model
{
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Model.Asset;
    using FluentAssertions;
    using Xunit;

    public class MapMeshMaterialTests
    {
        [Fact]
        public void ShouldContainExpectedValuesWhenCreated()
        {
            // Given
            var assetPath = "asset-path";
            var shaderId = "shader-id";
            var shader = "shader";

            var sandLimit = 1.1;
            var rockLimit = 2.2;
            var snowLimit = 3.3;

            var groundTexture = new TextureAsset
            {
                Image = "ground",
                VScale = 1,
                UScale = 2,
            };
            var grassTexture = new TextureAsset
            {
                Image = "grass",
                VScale = 2,
                UScale = 3,
            };
            var snowTexture = new TextureAsset
            {
                Image = "snow",
                VScale = 3,
                UScale = 4,
            };
            var sandTexture = new TextureAsset
            {
                Image = "sand",
                VScale = 4,
                UScale = 5,
            };
            var rockTexture = new TextureAsset
            {
                Image = "rock",
                VScale = 5,
                UScale = 6,
            };
            var blendTexture = new TextureAsset
            {
                Image = "blend",
            };

            // When
            var actual = new MapMeshMaterial
            {
                AssetPath = assetPath,
                ShaderId = shaderId,
                Shader = shader,
                SandLimit = sandLimit,
                RockLimit = rockLimit,
                SnowLimit = snowLimit,
                GroundTexture = groundTexture,
                GrassTexture = grassTexture,
                SnowTexture = snowTexture,
                SandTexture = sandTexture,
                RockTexture = rockTexture,
                BlendTexture = blendTexture,
            };

            // Then
            actual.AssetPath
                .Should().Be(assetPath);
            actual.ShaderId
                .Should().Be(shaderId);
            actual.Shader
                .Should().Be(shader);

            actual.SandLimit
                .Should().Be(sandLimit);
            actual.RockLimit
                .Should().Be(rockLimit);
            actual.SnowLimit
                .Should().Be(snowLimit);

            actual.GroundTexture
                .Should().Be(groundTexture);
            actual.GrassTexture
                .Should().Be(grassTexture);
            actual.SnowTexture
                .Should().Be(snowTexture);
            actual.SandTexture
                .Should().Be(sandTexture);
            actual.RockTexture
                .Should().Be(rockTexture);
            actual.BlendTexture
                .Should().Be(blendTexture);
        }
    }
}
