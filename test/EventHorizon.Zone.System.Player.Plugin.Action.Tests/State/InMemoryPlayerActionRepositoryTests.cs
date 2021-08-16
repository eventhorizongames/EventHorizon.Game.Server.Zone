using EventHorizon.Zone.System.Player.Plugin.Action.Model;
using EventHorizon.Zone.System.Player.Plugin.Action.State;

using Moq;

using Xunit;

using SystemAction = System.Action;

namespace EventHorizon.Zone.System.Player.Plugin.Action.Tests.State
{
    public class InMemoryPlayerActionRepositoryTests
    {
        [Fact]
        public void TestShouldBeAbleToFindPlayerActionEntityWhenOnActionNameMatches()
        {
            // Given
            var id = 1L;
            var actionName = "action-name";
            var actionEventMock = new Mock<PlayerActionEvent>();
            var expected = new PlayerActionEntity(
                id,
                actionName,
                actionEventMock.Object
            );

            // When
            var repository = new InMemoryPlayerActionRepository();

            Assert.Empty(
                repository.Where(
                    actionName
                )
            );

            repository.On(
                new PlayerActionEntity(
                    id,
                    actionName,
                    actionEventMock.Object
                )
            );
            var actual = repository.Where(
                actionName
            );

            // Then
            Assert.Collection(
                actual,
                action => Assert.Equal(expected, action)
            );
        }

        [Fact]
        public void TestShouldThrowHardExceptionWhenAlreadyContainsActionById()
        {
            // Given
            var id = 1L;
            var expected = id;
            var expectedMessage = "Please remove Player Action before adding another of the same Player Action Id.";
            var actionName = "action-name";
            var actionEventMock = new Mock<PlayerActionEvent>();
            var input = new PlayerActionEntity(
                id,
                actionName,
                actionEventMock.Object
            );

            // When
            var repository = new InMemoryPlayerActionRepository();

            Assert.Empty(
                repository.Where(
                    actionName
                )
            );

            repository.On(
                input
            );

            SystemAction action = () => repository.On(
                input
            );

            var actual = Record.Exception(
                action
            ) as AlreadyContainsPlayerAction;

            // Then
            Assert.Equal(
                expected,
                actual.ActionId
            );
            Assert.Equal(
                expectedMessage,
                actual.Message
            );
        }
    }
}
