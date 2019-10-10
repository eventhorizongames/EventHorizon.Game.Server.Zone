using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.System.Admin.Plugin.Command.Scripts.Load;
using EventHorizon.Zone.System.Admin.Plugin.Command.Scripts.State;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Tests.Scripts.Load
{
    public class LoadAdminCommandsHandlerTests
    {
        [Fact]
        public async Task TestShouldAddAdminCommandFromAdminCommandsPath()
        {
            // Given
            var expected = new AdminCommandInstance();
            var adminPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Scripts",
                "Load"
            );

            var serverInfoMock = new Mock<ServerInfo>();
            var repositoryMock = new Mock<AdminCommandRepository>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();

            serverInfoMock.Setup(
                mock => mock.AdminPath
            ).Returns(
                adminPath
            );

            // When
            var handler = new LoadAdminCommandsHandler(
                serverInfoMock.Object,
                repositoryMock.Object,
                fileLoaderMock.Object
            );
            await handler.Handle(
                new LoadAdminCommands(),
                CancellationToken.None
            );

            // Then
            repositoryMock.Verify(
                mock => mock.Add(
                    It.IsAny<AdminCommandInstance>()
                ),
                Times.Exactly(1)
            );
            repositoryMock.Verify(
                mock => mock.Add(
                    expected
                )
            );
        }
    }
}