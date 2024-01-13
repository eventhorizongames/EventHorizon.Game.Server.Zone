namespace EventHorizon.Zone.System.Agent.Plugin.Move.Tests.Repository;

using EventHorizon.Zone.System.Agent.Move.Repository;

using Xunit;

public class MoveAgentRepositoryTests
{
    [Fact]
    public void Test_ShouldAddAgentEntityIdToRepositoryButNotDequeueable()
    {
        // Given
        var expected = false;
        var inputId1 = 123L;

        // When
        var moveAgentRepository = new MoveAgentRepository();
        moveAgentRepository.Register(
            inputId1
        );

        // Then
        var actual = moveAgentRepository.Dequeue(
            out inputId1
        );

        Assert.Equal(
            expected,
            actual
        );
        Assert.Equal(
            0,
            inputId1
        );
    }
    [Fact]
    public void Test_ShouldBeAbleToDequeueRegisteredIdAfterMergeIsCalled()
    {
        // Given
        var expectedId = 123;
        var inputId1 = 123;

        // When
        var moveAgentRepository = new MoveAgentRepository();
        moveAgentRepository.Register(
            inputId1
        );
        moveAgentRepository.MergeRegisteredIntoQueue();
        var actual = moveAgentRepository.Dequeue(
            out var actualId
        );

        // Then
        Assert.True(
            actual
        );
        Assert.Equal(
            expectedId,
            actualId
        );
    }
}
