using System.Threading.Tasks;
using EventHorizon.Zone.System.Interaction.Model.Client;
using Xunit;

namespace EventHorizon.Zone.System.Interaction.Tests.Model.Client
{
    public class InteractionClientActionDataTests
    {
        [Fact]
        public void TestShouldBeAbleToAccessCommandTypeAndData()
        {
            // Given
            var expectedCommandType = "command-type";
            var expectedData = new { TestArgument = "hello-world" };

            // When
            var actual = new InteractionClientActionData(
                expectedCommandType,
                expectedData
            );

            // Then
            Assert.Equal(
                expectedCommandType,
                actual.CommandType
            );
            Assert.Equal(
                expectedData,
                actual.Data
            );
        }
    }
}