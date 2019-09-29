using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Interaction.Script.Load;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Interaction.Tests.Script.Load
{
    public class LoadInteractionScriptsCommandHandlerTests
    {
        [Fact]
        public async Task TestShouldRegisterScriptsFromServerScriptsPath()
        {
            // Given
            var expectedTestDataList = new List<TestData>()
            {
                new TestData(
                    "SubLoadedScript.csx",
                    "Interaction\\SubDirectory",
                    "// Sub Script Comment"
                ),
                new TestData(
                    "LoadedScript.csx",
                    "Interaction",
                    "// Script Comment"
                ),
            };
            var expectedReferenceAssemblies = typeof(LoadInteractionScriptsCommandHandler).Assembly;
            var testingScriptsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Script",
                "Load"
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                testingScriptsPath
            );

            // When
            var handler = new LoadInteractionScriptsCommandHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );

            await handler.Handle(
                new LoadInteractionScriptsCommand(),
                CancellationToken.None
            );

            for (int i = 0; i < mediatorMock.Invocations.Count; i++)
            {
                var expected = expectedTestDataList[i];
                var actualRegisterCommand = (RegisterServerScriptCommand)mediatorMock.Invocations[i].Arguments[0];

                // Then
                Assert.Equal(
                    expected.FileName,
                    actualRegisterCommand.FileName
                );
                Assert.Equal(
                    expected.FilePath,
                    actualRegisterCommand.Path
                );
                Assert.Equal(
                    expected.FileContent,
                    actualRegisterCommand.ScriptString
                );
                Assert.Collection(
                    actualRegisterCommand.ReferenceAssemblies,
                    item => Assert.Equal(
                        expectedReferenceAssemblies,
                        item
                    )
                );
                Assert.Empty(
                    actualRegisterCommand.Imports
                );
            }
        }

        struct TestData
        {
            public string FileName { get; }
            public string FilePath { get; }
            public string FileContent { get; }

            public TestData(
                string fileName,
                string filePath,
                string fileContent
            )
            {
                this.FileName = fileName;
                this.FilePath = filePath;
                this.FileContent = fileContent;
            }
        }
    }
}