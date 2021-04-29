namespace EventHorizon.Zone.System.Server.Scripts.Tests.Validation
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Consolidate;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Create;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using EventHorizon.Zone.System.Server.Scripts.Validation;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;


    public class NeedToCompileServerScriptsHandlerTests
    {
        [Fact]
        public async Task ShouldReturnFalseResultWhenCurrentHashMatchesCreatedContentHash()
        {
            // Given
            var currentHash = "current-hash";
            var scriptDetailsList = new List<ServerScriptDetails>();
            var consolidatedScripts = "consolidated-scripts";

            var mediatorMock = new Mock<IMediator>();
            var scriptStateMock = new Mock<ServerScriptsState>();
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new ConsolidateServerScriptsCommand(
                        scriptDetailsList
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateServerScriptsResult>(
                    new ConsolidateServerScriptsResult(
                        null,
                        null,
                        consolidatedScripts
                    )
                )
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new CreateHashFromContentCommand(
                        consolidatedScripts
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<string>(
                    true,
                    currentHash
                )
            );

            detailsRepositoryMock.Setup(
                mock => mock.All
            ).Returns(
                scriptDetailsList
            );

            scriptStateMock.Setup(
                mock => mock.CurrentHash
            ).Returns(
                currentHash
            );

            // When
            var handler = new NeedToCompileServerScriptsHandler(
                mediatorMock.Object,
                scriptStateMock.Object,
                detailsRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new NeedToCompileServerScripts(),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
            actual.Result.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnErrorCodeWhenConsolidateScriptCommandFails()
        {
            // Given
            var errorCode = "error-code";
            var expected = errorCode;

            var mediatorMock = new Mock<IMediator>();
            var scriptStateMock = new Mock<ServerScriptsState>();
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ConsolidateServerScriptsCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateServerScriptsResult>(
                    errorCode
                )
            );

            // When
            var handler = new NeedToCompileServerScriptsHandler(
                mediatorMock.Object,
                scriptStateMock.Object,
                detailsRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new NeedToCompileServerScripts(),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnFalseResultWhenCreateHashCommand()
        {
            // Given
            var currentHash = "current-hash";
            var scriptDetailsList = new List<ServerScriptDetails>();
            var consolidatedScripts = "consolidated-scripts";
            var errorCode = "error-code";

            var expected = errorCode;

            var mediatorMock = new Mock<IMediator>();
            var scriptStateMock = new Mock<ServerScriptsState>();
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new ConsolidateServerScriptsCommand(
                        scriptDetailsList
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateServerScriptsResult>(
                    new ConsolidateServerScriptsResult(
                        null,
                        null,
                        consolidatedScripts
                    )
                )
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new CreateHashFromContentCommand(
                        consolidatedScripts
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<string>(
                    errorCode
                )
            );

            detailsRepositoryMock.Setup(
                mock => mock.All
            ).Returns(
                scriptDetailsList
            );

            scriptStateMock.Setup(
                mock => mock.CurrentHash
            ).Returns(
                currentHash
            );

            // When
            var handler = new NeedToCompileServerScriptsHandler(
                mediatorMock.Object,
                scriptStateMock.Object,
                detailsRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new NeedToCompileServerScripts(),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnTrueResultWhenCurrentHashDoesNotMatchCreatedContentHash()
        {
            // Given
            var currentHash = "current-hash";
            var createdHash = "created-hash";
            var scriptDetailsList = new List<ServerScriptDetails>();
            var consolidatedScripts = "consolidated-scripts";

            var mediatorMock = new Mock<IMediator>();
            var scriptStateMock = new Mock<ServerScriptsState>();
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new ConsolidateServerScriptsCommand(
                        scriptDetailsList
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateServerScriptsResult>(
                    new ConsolidateServerScriptsResult(
                        null,
                        null,
                        consolidatedScripts
                    )
                )
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new CreateHashFromContentCommand(
                        consolidatedScripts
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<string>(
                    true,
                    createdHash
                )
            );

            detailsRepositoryMock.Setup(
                mock => mock.All
            ).Returns(
                scriptDetailsList
            );

            scriptStateMock.Setup(
                mock => mock.CurrentHash
            ).Returns(
                currentHash
            );

            // When
            var handler = new NeedToCompileServerScriptsHandler(
                mediatorMock.Object,
                scriptStateMock.Object,
                detailsRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new NeedToCompileServerScripts(),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
            actual.Result.Should().BeTrue();
        }
    }
}
