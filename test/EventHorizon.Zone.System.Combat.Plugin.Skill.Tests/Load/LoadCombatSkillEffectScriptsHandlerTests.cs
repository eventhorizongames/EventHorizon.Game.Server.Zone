namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Load
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Load;
    using EventHorizon.Zone.System.Server.Scripts.Events.Register;
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

    public class LoadCombatSkillEffectScriptsHandlerTests
    {
        [Fact]
        public async Task ShouldRegisterServerScriptWhenFileInfoIsPickedUpAtPath()
        {
            // Given
            var serverScriptsPath = $"{Path.DirectorySeparatorChar}server-script-path";
            var expectedFileName = "file-name";
            var expectedPath = "directory-path";
            var expectedScriptString = "text-from-file";
            var expectedScriptReferenceAssembiles = new List<Assembly>
            {
                typeof(LoadCombatSkillEffectScriptsHandlerTests).Assembly,
            };
            var expectedImports = new List<string>();
            var expectedTagList = new string[]
            {
                "Type:SkillEffectScript",
            };
            var expectedFromDirectory = Path.Combine(
                serverScriptsPath,
                "Effects"
            );

            var name = expectedFileName;
            var directoryName = Path.Combine(
                serverScriptsPath,
                expectedPath
            );
            var fullName = "full-name";
            var extension = "extension";
            var rootPath = serverScriptsPath;

            var fileInfo = new StandardFileInfo(
                name,
                directoryName,
                fullName,
                extension
            );

            string fromDirectory = null;
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile = null;
            IDictionary<string, object> arguments = null;

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var systemAssemblyListMock = new Mock<SystemProvidedAssemblyList>();

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
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
            );

            systemAssemblyListMock.Setup(
                mock => mock.List
            ).Returns(
                expectedScriptReferenceAssembiles
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        fullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedScriptString
            );

            // When
            var handler = new LoadCombatSkillEffectScriptsHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                systemAssemblyListMock.Object
            );
            await handler.Handle(
                new LoadCombatSkillEffectScripts(),
                CancellationToken.None
            );

            // Setup the catching of the actual
            RegisterServerScriptCommand actual = default;

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<RegisterServerScriptCommand>(),
                    CancellationToken.None
                )
            ).Callback<IRequest<Unit>, CancellationToken>(
                (request, _) =>
                {
                    actual = (RegisterServerScriptCommand)request;
                }
            );

            // Call the OnProcessFile with FileInfo and Arguments
            await onProcessFile(
                fileInfo,
                arguments
            );

            // Then
            actual.Should().NotBe(default(RegisterServerScriptCommand));
            actual.FileName
                .Should().Be(expectedFileName);
            actual.Path
                .Should().Be(expectedPath);
            actual.ScriptString
                .Should().Be(expectedScriptString);
            actual.ReferenceAssemblies
                .Should().BeEquivalentTo(expectedScriptReferenceAssembiles);
            actual.Imports
                .Should().BeEquivalentTo(expectedImports);
            actual.TagList
                .Should().BeEquivalentTo(expectedTagList);

            fromDirectory.Should().Be(expectedFromDirectory);
        }
    }
}
