using EventHorizon.Server.Core.External.Connection.Model;
using Xunit;

namespace EventHorizon.Server.Core.External.Tests.Connection.Model
{
    public class RegisteredZoneDetailsTests
    {
        [Fact]
        public void TestShouldValidateIdWhenSet()
        {
            // Given
            var expected = "id-1000";

            // When
            var details = new RegisteredZoneDetails
            {
                Id = expected
            };
            
            // Then
            Assert.Equal(
                expected, 
                details.Id
            );
        }
    }
}