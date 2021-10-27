namespace EventHorizon.Zone.Core.Entity.Tests.Lifetime
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Entity.Lifetime;
    using EventHorizon.Zone.Core.Entity.Model;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;

    using FluentAssertions;

    using MediatR;

    using Moq;

    using Xunit;

    public class OnStartupSetupEntityCoreCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldCreateEntityDirectoryWhenDoesNotExist(
            // Given
            string appDataPath,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ServerInfo> serverInfoMock,
            OnStartupSetupEntityCoreCommandHandler handler
        )
        {
            var expected = new CreateDirectory(
                Path.Combine(
                    appDataPath,
                    EntityCoreConstants.EntityAppDataPath
                )
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var actual = await handler.Handle(
                new OnStartupSetupEntityCoreCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );

            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldIgnoreFailureResultsWhenWriteResourceToFileHasFailureErrorCode(
            // Given
            string errorCode,
            string appDataPath,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ServerInfo> serverInfoMock,
            OnStartupSetupEntityCoreCommandHandler handler
        )
        {
            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<WriteResourceToFile>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new StandardCommandResult(
                    errorCode
                )
            );

            // When
            var actual = await handler.Handle(
                new OnStartupSetupEntityCoreCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldNotCreateGuiDirectoryWhenAlreadyExisting(
            // Given
            string appDataPath,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ServerInfo> serverInfoMock,
            OnStartupSetupEntityCoreCommandHandler handler
        )
        {
            var entityPath = Path.Combine(
                appDataPath,
                EntityCoreConstants.EntityAppDataPath
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        entityPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var actual = await handler.Handle(
                new OnStartupSetupEntityCoreCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );

            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<CreateDirectory>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}
