using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.Register;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Server.Scripts.Tests.Register
{
    public class SystemRegisterServerScriptCommandHandlerTests
    {
        [Fact]
        public async Task TestShouldAddServerScriptWhenScriptIsHandled()
        {
            // Given
            var fileName = "file-name";
            var path = "path";
            var scriptString = "// A script";

            var mediatorMock = new Mock<IMediator>();
            var serverScriptRepositoryMock = new Mock<ServerScriptRepository>();

            // When
            var handler = new SystemRegisterServerScriptCommandHandler(
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
            var importList = new List<string>();
            var tagList = new List<string>();

            var mediatorMock = new Mock<IMediator>();
            var serverScriptRepositoryMock = new Mock<ServerScriptRepository>();

            // When
            var handler = new SystemRegisterServerScriptCommandHandler(
                mediatorMock.Object,
                serverScriptRepositoryMock.Object
            );
            await handler.Handle(
                new RegisterServerScriptCommand(
                    fileName,
                    path,
                    scriptString,
                    referenceAssemblyList,
                    importList,
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
                            registerEvent.Imports == importList
                            &&
                            registerEvent.TagList == tagList
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}