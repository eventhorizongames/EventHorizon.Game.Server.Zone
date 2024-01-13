namespace EventHorizon.Zone.Core.Tests.Id;

using System.Threading.Tasks;

using EventHorizon.Zone.Core.Id;

using Xunit;

public class InMemoryStaticIdPoolTests
{
    [Fact]
    public void TestShouldReturnIdsInOrderWhenNextIsRequested()
    {
        // Given
        var expected = new long[]
        {
            0,
            1,
            2,
            3,
            4
        };

        // When
        var idPool = new InMemoryStaticIdPool();

        // Then
        foreach (var expectedId in expected)
        {
            Assert.Equal(
                expectedId,
                idPool.NextId()
            );
        }
    }
}
