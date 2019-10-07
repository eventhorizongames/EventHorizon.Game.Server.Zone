using EventHorizon.Server.Core.External.Connection.Model;
using Xunit;

namespace EventHorizon.Server.Core.External.Tests.Connection.Model
{
    public class ZoneRegistrationDetailsTests
    {
        [Fact]
        public void TestShouldHaveValidatePropertiesWhenSet()
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