using System.Threading.Tasks;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.System.Admin.Plugin.Command.Scripts.State;
using Xunit;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Tests.Scripts.State
{
    public class AdminCommandInMemoryRepositoryTests
    {
        [Fact]
        public void TestShouldBeAbleToFindAdminInstanceWhenAddedToRepository()
        {
            // Given
            var command = "command";
            var commandInstance = new AdminCommandInstance
            {
                Command = command
            };

            // When
            var repository = new AdminCommandInMemoryRepository();
            repository.Add(
                commandInstance
            );
            var actual = repository.Where(
                command
            );

            // Then
            Assert.Collection(
                actual,
                actualCommand => Assert.Equal(
                    actualCommand,
                    commandInstance
                )
            );
        }

        [Fact]
        public void TestShouldClearAnyExistingCommandsWhenRepositoyIsCleared()
        {
            // Given
            var command = "command";
            var commandInstance = new AdminCommandInstance
            {
                Command = command
            };

            // When
            var repository = new AdminCommandInMemoryRepository();
            repository.Add(
                commandInstance
            );
            var validation = repository.Where(
                command
            );
            Assert.Collection(
                validation,
                actualCommand => Assert.Equal(
                    actualCommand,
                    commandInstance
                )
            );
            repository.Clear();
            var actual = repository.Where(
                command
            );

            // Then
            Assert.Empty(
                actual
            );
        }
    }
}