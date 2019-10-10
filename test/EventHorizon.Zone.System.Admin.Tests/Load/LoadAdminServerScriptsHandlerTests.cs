using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Admin.Load;
using EventHorizon.Zone.System.Server.Scripts.Events.Load;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Admin.Tests.Load
{
    public class LoadAdminServerScriptsHandlerTests
    {
        [Fact]
        public async Task TestName()
        {
            // Given
            var expectedTestDataList = new List<TestData>()
            {
                new TestData(
                    "SubLoadedScript.csx",
                    $"Admin{Path.DirectorySeparatorChar}SubDirectory",
                    "// Sub Script Comment"
                ),
                new TestData(
                    "LoadedScript.csx",
                    "Admin",
                    "// Script Comment"
                ),
            };
            var expectedReferenceAssemblies = typeof(LoadAdminServerScriptsHandlerTests).Assembly;
            var testingScriptsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Load"
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var systemAssemblyListMock = new Mock<SystemProvidedAssemblyList>();

            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                testingScriptsPath
            );

            systemAssemblyListMock.Setup(
                mock => mock.List
            ).Returns(
                new List<Assembly>
                {
                    expectedReferenceAssemblies
                }
            );

            // When
            var handler = new LoadAdminServerScriptsHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                systemAssemblyListMock.Object
            );

            await handler.Handle(
                new LoadServerScriptsCommand(),
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