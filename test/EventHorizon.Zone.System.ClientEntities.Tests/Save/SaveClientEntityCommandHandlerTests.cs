using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.ClientEntities.Client;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.Model.Client;
using EventHorizon.Zone.System.ClientEntities.Save;
using EventHorizon.Zone.System.ClientEntities.State;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.ClientEntities.Tests.Save
{
    public class SaveClientEntityCommandHandlerTests
    {
        [Fact]
        public async Task ShouldSendChangeClientActionToAllEventWhenSuccessfulySavedClientEntity()
        {
            // Given
            var clientEntityId = "client-entity";
            var fullName = "full-name";
            var clientEntity = new ClientEntity(
                clientEntityId,
                new Dictionary<string, object>
                {
                    { "editor:Metadata:FullName", fullName }
                }
            );
            var expected = new ClientEntityChangedClientActionData(
                clientEntity
            );

            var mediatorMock = new Mock<IMediator>();
            var fileSaverMock = new Mock<IJsonFileSaver>();
            var repositoryMock = new Mock<ClientEntityRepository>();

            repositoryMock.Setup(
                mock => mock.Find(
                    clientEntityId
                )
            ).Returns(
                clientEntity
            );

            // When
            var handler = new SaveClientEntityCommandHandler(
                mediatorMock.Object,
                fileSaverMock.Object,
                repositoryMock.Object
            );
            await handler.Handle(
                new SaveClientEntityCommand(
                    clientEntity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.Is<SendClientEntityChangedClientActionToAllEvent>(
                        evt => evt.Data.Equals(
                            expected
                        )
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldCreateBackupWhenFileInfoExistsForFullName()
        {
            // Given
            var clientEntityId = "client-entity";
            var fileName = "file-name";
            var fullName = "full-name";
            var clientEntity = new ClientEntity(
                clientEntityId,
                new Dictionary<string, object>
                {
                    { "editor:Metadata:FullName", fullName }
                }
            );
            var expectedPath = new string[] { "Client", "Entity" };
            var expectedFileName = fileName;
            var expectedFileText = "file-text";

            var mediatorMock = new Mock<IMediator>();
            var fileSaverMock = new Mock<IJsonFileSaver>();
            var repositoryMock = new Mock<ClientEntityRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetFileInfo(
                        fullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new StandardFileInfo(
                    fileName,
                    "file-director",
                    fullName,
                    "json"
                )
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesFileExist(
                        fullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        fullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedFileText
            );

            repositoryMock.Setup(
                mock => mock.Find(
                    clientEntityId
                )
            ).Returns(
                clientEntity
            );

            // When
            var handler = new SaveClientEntityCommandHandler(
                mediatorMock.Object,
                fileSaverMock.Object,
                repositoryMock.Object
            );
            await handler.Handle(
                new SaveClientEntityCommand(
                    clientEntity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<CreateBackupOfFileContentCommand>(
                        actual => actual.FilePath.Contains(expectedPath[0])
                            && actual.FilePath.Contains(expectedPath[1])
                            && actual.FileName == expectedFileName
                            && actual.FileContent == expectedFileText
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldBubbleExceptionWhenAnyExceptionIsThrown()
        {
            // Given
            var expected = "Object reference not set to an instance of an object.";
            var clientEntity = new ClientEntity(
                "client-id",
                null
            );

            var mediatorMock = new Mock<IMediator>();
            var fileSaverMock = new Mock<IJsonFileSaver>();
            var repositoryMock = new Mock<ClientEntityRepository>();

            // When
            var handler = new SaveClientEntityCommandHandler(
                mediatorMock.Object,
                fileSaverMock.Object,
                repositoryMock.Object
            );
            Func<Task> action = () => handler.Handle(
                new SaveClientEntityCommand(
                    clientEntity
                ),
                CancellationToken.None
            );
            var actual = await Assert.ThrowsAsync<NullReferenceException>(
                action
            );

            // Then
            Assert.Equal(
                expected,
                actual.Message
            );
        }
    }
}