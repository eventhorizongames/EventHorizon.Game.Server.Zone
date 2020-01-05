using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;
using EventHorizon.Zone.System.Server.Scripts.State;
using Moq;
using Xunit;
using EventHorizon.Zone.System.Server.Scripts.Register;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using System.Reflection;
using System.Threading;

namespace EventHorizon.Zone.System.Server.Scripts.Tests.Register
{
    public class SystemServerScriptRegisteredEventHandlerTests
    {
        [Fact]
        public async Task TestShouldAddServerScriptDetailsToRepositoryWhenHandlingNotificationEvent()
        {
            // Given
            var assembly = typeof(SystemServerScriptRegisteredEventHandlerTests).Assembly;
            var expectedScriptId = "script-id";
            var expectedFileName = "file-name";
            var expectedPath = "path";
            var expectedScriptString = "script-string";
            var expectedAssembly = assembly.ToString();
            var expectedImportList = new List<string>();
            var expectedTagList = new List<string>();

            var serverScriptDetailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();

            // When
            var handler = new SystemServerScriptRegisteredEventHandler(
                serverScriptDetailsRepositoryMock.Object
            );
            await handler.Handle(
                new ServerScriptRegisteredEvent(
                    expectedScriptId,
                    expectedFileName,
                    expectedPath,
                    expectedScriptString,
                    new List<Assembly>
                    {
                        assembly
                    },
                    expectedImportList,
                    expectedTagList
                ),
                CancellationToken.None
            );

            // Then
            serverScriptDetailsRepositoryMock.Verify(
                mock => mock.Add(
                    expectedScriptId,
                    It.Is<ServerScriptDetails>(
                        scriptDetails =>
                            scriptDetails.Id == expectedScriptId
                            &&
                            scriptDetails.FileName == expectedFileName
                            &&
                            scriptDetails.Path == expectedPath
                            &&
                            scriptDetails.ScriptString == expectedScriptString
                            &&
                            scriptDetails.ReferenceAssemblies.Contains(
                                expectedAssembly
                            )
                            &&
                            scriptDetails.Imports == expectedImportList
                            &&
                            scriptDetails.TagList == expectedTagList
                    )
                )
            );
        }
    }
}