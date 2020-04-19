namespace EventHorizon.Zone.System.ClientEntities.Tests.Model
{
    using global::System;
    using global::System.Numerics;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using Xunit;

    public class ClientEntityMetadataTypesTests
    {
        [Fact]
        public void ShouldHaveExpectedTypes()
        {
            Assert.True(
                ClientEntityMetadataTypes.TYPE_DETAILS.assetId == typeof(string)
            );
            Assert.True(
                ClientEntityMetadataTypes.TYPE_DETAILS.dense == typeof(bool)
            );
            Assert.True(
                ClientEntityMetadataTypes.TYPE_DETAILS.densityBox == typeof(Nullable<Vector3>)
            );
            Assert.True(
                ClientEntityMetadataTypes.TYPE_DETAILS.resolveHeight == typeof(bool)
            );
            Assert.True(
                ClientEntityMetadataTypes.TYPE_DETAILS.heightOffset == typeof(long)
            );
        }
    }
}