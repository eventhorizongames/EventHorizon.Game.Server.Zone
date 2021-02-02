namespace EventHorizon.Zone.System.Server.Scripts.Tests.Register
{
    using EventHorizon.Zone.System.Server.Scripts.Events.Register;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Register;
    using global::System.Collections.Generic;
    using global::System.Reflection;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class SystemRegisterServerScriptCommandHandlerTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestShouldAddServerScriptWhenScriptIsHandled()
        {
            // Given
            var fileName = "file-name";
            var path = "path";
            var scriptString = "// A script";

            var loggerMock = new Mock<ILogger<SystemRegisterServerScriptCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverScriptRepositoryMock = new Mock<ServerScriptRepository>();

            // When
            var handler = new SystemRegisterServerScriptCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverScriptRepositoryMock.Object
            );
            await handler.Handle(
                new RegisterServerScriptCommand(
                    fileName,
                    path,
                    scriptString
                ),
                CancellationToken.None
            );

            // Then
            serverScriptRepositoryMock.Verify(
                mock => mock.Add(
                    It.IsAny<ServerScript>()
                )
            );
        }

        [Fact]
        public async Task TestShouldPublishRegisteredEventWhenTheScriptIsFinishedRegistering()
        {
            // Given
            var fileName = "file-name";
            var path = "path";
            var scriptString = "// A script";
            var referenceAssemblyList = new List<Assembly>();
            var tagList = new List<string>();

            var loggerMock = new Mock<ILogger<SystemRegisterServerScriptCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverScriptRepositoryMock = new Mock<ServerScriptRepository>();

            // When
            var handler = new SystemRegisterServerScriptCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverScriptRepositoryMock.Object
            );
            await handler.Handle(
                new RegisterServerScriptCommand(
                    fileName,
                    path,
                    scriptString,
                    referenceAssemblyList,
                    tagList
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.Is<ServerScriptRegisteredEvent>(
                        registerEvent =>
                            !string.IsNullOrEmpty(
                                registerEvent.Id
                            )
                            &&
                            registerEvent.FileName == fileName
                            &&
                            registerEvent.Path == path
                            &&
                            registerEvent.ScriptString == scriptString
                            &&
                            registerEvent.ReferenceAssemblies == referenceAssemblyList
                            &&
                            registerEvent.TagList == tagList
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}