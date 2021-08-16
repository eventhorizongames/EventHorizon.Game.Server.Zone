namespace EventHorizon.Zone.System.Client.Scripts.Tests.Validation
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Consolidate;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Create;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Model;
    using EventHorizon.Zone.System.Client.Scripts.Validation;

    using FluentAssertions;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class NeedToCompileClientScriptsHandlerTests
    {
        [Fact]
        public async Task ShouldReturnFalseResultWhenCurrentHashMatchesCreatedContentHash()
        {
            // Given
            var currentHash = "current-hash";
            var scriptDetailsList = new List<ClientScript>();
            var consolidatedScripts = "consolidated-scripts";

            var mediatorMock = new Mock<IMediator>();
            var detailsRepositoryMock = new Mock<ClientScriptRepository>();
            var scriptStateMock = new Mock<ClientScriptsState>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ConsolidateClientScriptsCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateClientScriptsResult>(
                    new ConsolidateClientScriptsResult(
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
                mock => mock.All()
            ).Returns(
                scriptDetailsList
            );

            scriptStateMock.Setup(
                mock => mock.Hash
            ).Returns(
                currentHash
            );

            // When
            var handler = new NeedToCompileClientScriptsHandler(
                mediatorMock.Object,
                detailsRepositoryMock.Object,
                scriptStateMock.Object
            );
            var actual = await handler.Handle(
                new NeedToCompileClientScripts(),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();
            actual.Result
                .Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnErrorCodeWhenConsolidateScriptCommandFails()
        {
            // Given
            var errorCode = "error-code";
            var expected = errorCode;

            var mediatorMock = new Mock<IMediator>();
            var detailsRepositoryMock = new Mock<ClientScriptRepository>();
            var scriptStateMock = new Mock<ClientScriptsState>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ConsolidateClientScriptsCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateClientScriptsResult>(
                    errorCode
                )
            );

            // When
            var handler = new NeedToCompileClientScriptsHandler(
                mediatorMock.Object,
                detailsRepositoryMock.Object,
                scriptStateMock.Object
            );
            var actual = await handler.Handle(
                new NeedToCompileClientScripts(),
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
            var scriptDetailsList = new List<ClientScript>();
            var consolidatedScripts = "consolidated-scripts";
            var errorCode = "error-code";

            var expected = errorCode;

            var mediatorMock = new Mock<IMediator>();
            var detailsRepositoryMock = new Mock<ClientScriptRepository>();
            var scriptStateMock = new Mock<ClientScriptsState>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ConsolidateClientScriptsCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateClientScriptsResult>(
                    new ConsolidateClientScriptsResult(
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
                mock => mock.All()
            ).Returns(
                scriptDetailsList
            );

            scriptStateMock.Setup(
                mock => mock.Hash
            ).Returns(
                currentHash
            );

            // When
            var handler = new NeedToCompileClientScriptsHandler(
                mediatorMock.Object,
                detailsRepositoryMock.Object,
                scriptStateMock.Object
            );
            var actual = await handler.Handle(
                new NeedToCompileClientScripts(),
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
            var scriptDetailsList = new List<ClientScript>();
            var consolidatedScripts = "consolidated-scripts";

            var mediatorMock = new Mock<IMediator>();
            var scriptStateMock = new Mock<ClientScriptsState>();
            var detailsRepositoryMock = new Mock<ClientScriptRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ConsolidateClientScriptsCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateClientScriptsResult>(
                    new ConsolidateClientScriptsResult(
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
                mock => mock.All()
            ).Returns(
                scriptDetailsList
            );

            scriptStateMock.Setup(
                mock => mock.Hash
            ).Returns(
                currentHash
            );

            // When
            var handler = new NeedToCompileClientScriptsHandler(
                mediatorMock.Object,
                detailsRepositoryMock.Object,
                scriptStateMock.Object
            );
            var actual = await handler.Handle(
                new NeedToCompileClientScripts(),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();
            actual.Result
                .Should().BeTrue();
        }
    }
}
