namespace EventHorizon.Zone.System.Particle.Tests.Load
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Particle.Events.Add;
    using EventHorizon.Zone.System.Particle.Load;
    using EventHorizon.Zone.System.Particle.Model.Template;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class LoadParticleSystemEventHandlerTests
    {
        [Fact]
        public async Task TestShouldRegisterScriptsFromServerScriptsPath()
        {
            // Given
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile = null;
            IDictionary<string, object> arguments = null;
            var clientPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "client-path"
            );
            var clientParticlePath = Path.Combine(
                clientPath,
                "Particle"
            );
            var fileExtension = ".json";
            var particleFileName = "Particle.json";
            var particleFileFullName = Path.Combine(
                clientParticlePath,
                particleFileName
            );
            var particleDirectoryFullName = Path.Combine(
                clientParticlePath
            );
            var particleFileInfo = new StandardFileInfo(
                particleFileName,
                particleDirectoryFullName,
                particleFileFullName,
                fileExtension
            );
            var particleFileContent = "particle-file-content";

            var particleName = "particle-name";
            var particleTemplate = new ParticleTemplate
            {
                Name = particleName,
            };

            var expectedParticleId = "Particle_Particle";
            var expectedParticle = new ParticleTemplate
            {
                Id = expectedParticleId,
                Name = particleName,
            };

            var mediatorMock = new Mock<IMediator>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );

            fileLoaderMock.Setup(
                mock => mock.GetFile<ParticleTemplate>(
                    particleFileFullName
                )
            ).ReturnsAsync(
                particleTemplate
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ProcessFilesRecursivelyFromDirectory>(),
                    CancellationToken.None
                )
            ).Callback<IRequest<Unit>, CancellationToken>(
                (evt, token) =>
                {
                    onProcessFile = ((ProcessFilesRecursivelyFromDirectory)evt).OnProcessFile;
                    arguments = ((ProcessFilesRecursivelyFromDirectory)evt).Arguments;
                }
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        particleFileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                particleFileContent
            );

            // When
            var handler = new LoadParticleSystemEventHandler(
                mediatorMock.Object,
                fileLoaderMock.Object,
                serverInfoMock.Object
            );

            await handler.Handle(
                new LoadParticleSystemEvent(),
                CancellationToken.None
            );
            Assert.NotNull(
                onProcessFile
            );

            await onProcessFile(
                particleFileInfo,
                arguments
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    new AddParticleTemplateEvent(
                        expectedParticleId,
                        expectedParticle
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
