namespace EventHorizon.Server.Core.Connection.Tests.Model
{
    using EventHorizon.Server.Core.Connection.Model;
    using Xunit;

    public class ZoneRegistrationDetailsTests
    {
        [Fact]
        public void ShouldHaveValidatePropertiesWhenSet()
        {
            // Given
            var expectedServerAddress = "server-address";
            var expectedTag = "tag";

            // When
            var details = new ZoneRegistrationDetails
            {
                ServerAddress = expectedServerAddress,
                Tag = expectedTag
            };

            // Then
            Assert.Equal(
                expectedServerAddress,
                details.ServerAddress
            );
            Assert.Equal(
                expectedTag,
                details.Tag
            );
        }
    }
}