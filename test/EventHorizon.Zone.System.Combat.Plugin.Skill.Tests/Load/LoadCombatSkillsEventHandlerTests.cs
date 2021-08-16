namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Load
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Load;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;

    using FluentAssertions;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Reflection;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class LoadCombatSkillsEventHandlerTests
    {
        [Fact]
        public async Task ShouldSetSkillLoadedWhenLoadedFromFileLoaderOnProcessingFile()
        {
            // Given
            var clientPath = $"{Path.DirectorySeparatorChar}client-path";
            var path = "directory-path";

            var expectedFromDirectory = Path.Combine(
                clientPath,
                "Skills"
            );
            var expected = new SkillInstance()
            {
                Id = "directory-path_file-name",
            };

            var name = "file-name";
            var directoryName = Path.Combine(
                clientPath,
                path
            );
            var fullName = "full-name";
            var extension = "extension";
            var rootPath = clientPath;

            var fileInfo = new StandardFileInfo(
                name,
                directoryName,
                fullName,
                extension
            );
            var skillInstance = new SkillInstance();

            string fromDirectory = null;
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile = null;
            IDictionary<string, object> arguments = null;

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var skillRepositoryMock = new Mock<SkillRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ProcessFilesRecursivelyFromDirectory>(),
                    CancellationToken.None
                )
            ).Callback<IRequest<Unit>, CancellationToken>(
                (rawRequest, _) =>
                {
                    var request = (ProcessFilesRecursivelyFromDirectory)rawRequest;
                    fromDirectory = request.FromDirectory;
                    onProcessFile = request.OnProcessFile;
                    arguments = request.Arguments;
                }
            );

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );

            fileLoaderMock.Setup(
                mock => mock.GetFile<SkillInstance>(
                    fullName
                )
            ).ReturnsAsync(
                skillInstance
            );

            // When
            var handler = new LoadCombatSkillsEventHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                fileLoaderMock.Object,
                skillRepositoryMock.Object
            );
            await handler.Handle(
                new LoadCombatSkillsEvent(),
                CancellationToken.None
            );

            // Setup the catching of the actual
            var actual = default(SkillInstance);

            skillRepositoryMock.Setup(
                mock => mock.Set(
                    It.IsAny<SkillInstance>()
                )
            ).Callback<SkillInstance>(
                (skillInstance) =>
                {
                    actual = skillInstance;
                }
            );

            // Call the OnProcessFile with FileInfo and Arguments
            await onProcessFile(
                fileInfo,
                arguments
            );

            // Then
            fromDirectory.Should().Be(expectedFromDirectory);
            actual.Should().Be(expected);
        }
    }
}
