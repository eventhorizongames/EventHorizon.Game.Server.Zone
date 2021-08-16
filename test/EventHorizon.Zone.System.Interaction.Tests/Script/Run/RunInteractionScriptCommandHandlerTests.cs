using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Interaction.Model;
using EventHorizon.Zone.System.Interaction.Script.Run;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;

using MediatR;

using Moq;

using Xunit;

using static EventHorizon.Zone.System.Interaction.Script.Run.RunInteractionScriptCommandHandler;

namespace EventHorizon.Zone.System.Interaction.Tests.Script.Run
{
    public class RunInteractionScriptCommandHandlerTests
    {
        [Fact]
        public async Task TestShouldRunServerScriptCommandWithExpectedDataArgument()
        {
            // Given
            var expectedScriptId = "script-id";
            var interactionItem = new InteractionItem()
            {
                ScriptId = expectedScriptId
            };
            var player = new PlayerEntity();
            var interactionEntity = new DefaultEntity();
            var expectedDictionaryArguments = new TestData[] {
                new TestData(
                    "Interaction",
                    interactionItem
                ),
                new TestData(
                    "Player",
                    player
                ),
                new TestData(
                    "Target",
                    interactionEntity
                ),
            };

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new RunInteractionScriptCommandHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new RunInteractionScriptCommand(
                    interactionItem,
                    interactionEntity,
                    player
                ),
                CancellationToken.None
            );
            var actualCommand = (RunServerScriptCommand)mediatorMock.Invocations[0].Arguments[0];

            // Then
            Assert.Equal(
                typeof(InternalRunInteractionScriptResponse),
                actual.GetType()
            );
            Assert.Equal(
                expectedScriptId,
                actualCommand.Id
            );
            foreach (var argument in expectedDictionaryArguments)
            {
                Assert.True(actualCommand.Data.ContainsKey(argument.Name));
                Assert.Equal(
                    actualCommand.Data[argument.Name],
                    argument.Data
                );
            }
        }

        struct TestData
        {
            public string Name { get; }
            public object Data { get; }

            public TestData(
                string argumentName,
                object argumentData
            )
            {
                Name = argumentName;
                Data = argumentData;
            }
        }
    }
}
