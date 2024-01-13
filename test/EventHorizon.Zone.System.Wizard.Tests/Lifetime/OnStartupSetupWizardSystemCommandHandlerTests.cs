namespace EventHorizon.Zone.System.Wizard.Tests.Lifetime;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Lifetime;
using EventHorizon.Zone.System.Wizard.Lifetime;

using FluentAssertions;

using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class OnStartupSetupWizardSystemCommandHandlerTests
{
    [Fact]
    public async Task ShouldCreateBehaviorDirectoriesWhenDoesNotExist()
    {
        // Given
        var serverPath = "server-path";
        var expected = new CreateDirectory(
            Path.Combine(
                serverPath,
                "Wizards"
            )
        );

        var loggerMock = new Mock<ILogger<OnStartupSetupWizardSystemCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();

        serverInfoMock.Setup(
            mock => mock.ServerPath
        ).Returns(
            serverPath
        );

        // When
        var handler = new OnStartupSetupWizardSystemCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object
        );
        var actual = await handler.Handle(
            new OnStartupSetupWizardSystemCommand(),
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

    [Fact]
    public async Task ShouldNotCreateBehaviorDirectoriesWhenAlreadyExisting()
    {
        // Given
        var serverPath = "server-path";
        var wizardsPath = Path.Combine(
            serverPath,
            "Wizards"
        );

        var loggerMock = new Mock<ILogger<OnStartupSetupWizardSystemCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();

        serverInfoMock.Setup(
            mock => mock.ServerPath
        ).Returns(
            serverPath
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new DoesDirectoryExist(
                    wizardsPath
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        // When
        var handler = new OnStartupSetupWizardSystemCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object
        );
        var actual = await handler.Handle(
            new OnStartupSetupWizardSystemCommand(),
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
