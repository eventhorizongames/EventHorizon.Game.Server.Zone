namespace EventHorizon.Zone.System.ClientEntities.Tests.Save
{
    using AutoFixture.Xunit2;

    using Castle.Core.Logging;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;
    using EventHorizon.Zone.System.ClientEntities.Save;
    using EventHorizon.Zone.System.ClientEntities.State;

    using FluentAssertions;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;
using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class SaveClientEntityCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldReturnNewlyRegisteredEntityWhenSuccessfulySaved(
            // Given
            string clientEntityId,
            string fullName,
            [Frozen] Mock<ClientEntityRepository> repositoryMock,
            SaveClientEntityCommandHandler handler
        )
        {
            var clientEntity = new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { "editor:Metadata:FullName", fullName }
                    }
                )
            );
            var expected = clientEntity;

            repositoryMock.Setup(
                mock => mock.Find(
                    clientEntityId
                )
            ).Returns(
                clientEntity
            );

            // When
            var actual = await handler.Handle(
                new SaveClientEntityCommand(
                    clientEntity
                ),
                CancellationToken.None
            );

            // Then
            actual.ClientEntity.Should()
                .Be(
                    expected
                );
        }

        //[Fact]
        //public async Task ShouldSendChangeClientActionToAllEventWhenSuccessfulySavedClientEntity()
        //{
        //    // Given
        //    var clientEntityId = "client-entity";
        //    var fullName = "full-name";
        //    var clientEntity = new ClientEntity(
        //        clientEntityId,
        //        new ConcurrentDictionary<string, object>(
        //            new Dictionary<string, object>
        //            {
        //                { "editor:Metadata:FullName", fullName }
        //            }
        //        )
        //    );
        //    var expected = new ClientEntityChangedClientActionData(
        //        clientEntity
        //    );

        //    var mediatorMock = new Mock<IMediator>();
        //    var fileSaverMock = new Mock<IJsonFileSaver>();
        //    var repositoryMock = new Mock<ClientEntityRepository>();

        //    repositoryMock.Setup(
        //        mock => mock.Find(
        //            clientEntityId
        //        )
        //    ).Returns(
        //        clientEntity
        //    );

        //    // When
        //    var handler = new SaveClientEntityCommandHandler(
        //        mediatorMock.Object,
        //        fileSaverMock.Object,
        //        repositoryMock.Object
        //    );
        //    await handler.Handle(
        //        new SaveClientEntityCommand(
        //            clientEntity
        //        ),
        //        CancellationToken.None
        //    );

        //    // Then
        //    mediatorMock.Verify(
        //        mock => mock.Publish(
        //            It.Is<ClientActionGenericToAllEvent>(
        //                evt => evt.Data.Equals(
        //                    expected
        //                )
        //            ),
        //            CancellationToken.None
        //        )
        //    );
        //}

        //[Fact]
        //public async Task ShouldCreateBackupWhenFileInfoExistsForFullName()
        //{
        //    // Given
        //    var clientEntityId = "client-entity";
        //    var fileName = "file-name";
        //    var fullName = "full-name";
        //    var clientEntity = new ClientEntity(
        //        clientEntityId,
        //        new ConcurrentDictionary<string, object>(
        //            new Dictionary<string, object>
        //            {
        //                { "editor:Metadata:FullName", fullName }
        //            }
        //        )
        //    );
        //    var expectedPath = new string[] { "Client", "Entity" };
        //    var expectedFileName = fileName;
        //    var expectedFileText = "file-text";

        //    var mediatorMock = new Mock<IMediator>();
        //    var fileSaverMock = new Mock<IJsonFileSaver>();
        //    var repositoryMock = new Mock<ClientEntityRepository>();

        //    mediatorMock.Setup(
        //        mock => mock.Send(
        //            new GetFileInfo(
        //                fullName
        //            ),
        //            CancellationToken.None
        //        )
        //    ).ReturnsAsync(
        //        new StandardFileInfo(
        //            fileName,
        //            "file-director",
        //            fullName,
        //            "json"
        //        )
        //    );
        //    mediatorMock.Setup(
        //        mock => mock.Send(
        //            new DoesFileExist(
        //                fullName
        //            ),
        //            CancellationToken.None
        //        )
        //    ).ReturnsAsync(
        //        true
        //    );
        //    mediatorMock.Setup(
        //        mock => mock.Send(
        //            new ReadAllTextFromFile(
        //                fullName
        //            ),
        //            CancellationToken.None
        //        )
        //    ).ReturnsAsync(
        //        expectedFileText
        //    );

        //    repositoryMock.Setup(
        //        mock => mock.Find(
        //            clientEntityId
        //        )
        //    ).Returns(
        //        clientEntity
        //    );

        //    // When
        //    var handler = new SaveClientEntityCommandHandler(
        //        mediatorMock.Object,
        //        fileSaverMock.Object,
        //        repositoryMock.Object
        //    );
        //    await handler.Handle(
        //        new SaveClientEntityCommand(
        //            clientEntity
        //        ),
        //        CancellationToken.None
        //    );

        //    // Then
        //    mediatorMock.Verify(
        //        mock => mock.Send(
        //            It.Is<CreateBackupOfFileContentCommand>(
        //                actual => actual.FilePath.Contains(expectedPath[0])
        //                    && actual.FilePath.Contains(expectedPath[1])
        //                    && actual.FileName == expectedFileName
        //                    && actual.FileContent == expectedFileText
        //            ),
        //            CancellationToken.None
        //        )
        //    );
        //}

        //[Fact]
        //public async Task ShouldReturnFailedResponseWithErrorCodeWhenExceptionIsThrown()
        //{
        //    // Given
        //    var expected = "exception";
        //    var clientEntity = new ClientEntity(
        //        "client-id",
        //        null
        //    );

        //    var mediatorMock = new Mock<IMediator>();
        //    var fileSaverMock = new Mock<IJsonFileSaver>();
        //    var repositoryMock = new Mock<ClientEntityRepository>();

        //    // When
        //    var handler = new SaveClientEntityCommandHandler(
        //        mediatorMock.Object,
        //        fileSaverMock.Object,
        //        repositoryMock.Object
        //    );
        //    var actual = await handler.Handle(
        //        new SaveClientEntityCommand(
        //            clientEntity
        //        ),
        //        CancellationToken.None
        //    );

        //    // Then
        //    Assert.False(
        //        actual.Success
        //    );
        //    Assert.Equal(
        //        expected,
        //        actual.ErrorCode
        //    );
        //}
    }
}
