namespace EventHorizon.Zone.System.Client.Scripts.Tests.Compile
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Compile;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Model.Client;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class CompileClientScriptCommandHandlerTests
    {
        [Fact]
        public async Task ShouldNotCompileScriptsWhenNoCSharpScriptsAreInRepository()
        {
            // Given
            var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var clientScriptsStateMock = new Mock<ClientScriptsState>();
            var clientScriptRepositoryMock = new Mock<ClientScriptRepository>();
            var clientScriptCompilerMock = new Mock<ClientScriptCompiler>();

            // When
            var handler = new CompileClientScriptCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                clientScriptsStateMock.Object,
                clientScriptRepositoryMock.Object,
                clientScriptCompilerMock.Object
            );
            await handler.Handle(
                new CompileClientScriptCommand(),
                CancellationToken.None
            );

            // Then
            clientScriptCompilerMock.Verify(
                mock => mock.Compile(
                    It.IsAny<IEnumerable<ClientScript>>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldComplieScriptsWhenAnyScriptAreFoundInRepository()
        {
            // Given
            var expected = ClientScript.Create(
                ClientScriptType.CSharp,
                "path",
                "file-name",
                "script-string"
            );
            var expectedList = new List<ClientScript>
            {
                expected
            };
            var repositoryAll = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    "path",
                    "file-name",
                    "script-string"
                ),
                ClientScript.Create(
                    ClientScriptType.Unknown,
                    "path",
                    "file-name",
                    "script-string"
                ),
            };

            var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var clientScriptsStateMock = new Mock<ClientScriptsState>();
            var clientScriptRepositoryMock = new Mock<ClientScriptRepository>();
            var clientScriptCompilerMock = new Mock<ClientScriptCompiler>();

            clientScriptRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                repositoryAll
            );

            // When
            var handler = new CompileClientScriptCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                clientScriptsStateMock.Object,
                clientScriptRepositoryMock.Object,
                clientScriptCompilerMock.Object
            );
            var actual = default(IEnumerable<ClientScript>);
            clientScriptCompilerMock.Setup(
                mock => mock.Compile(
                    It.IsAny<IEnumerable<ClientScript>>()
                )
            ).Callback<IEnumerable<ClientScript>>(
                arg1 =>
                {
                    actual = arg1;
                }
            );
            await handler.Handle(
                new CompileClientScriptCommand(),
                CancellationToken.None
            );


            // Then
            clientScriptCompilerMock.Verify(
                mock => mock.Compile(
                    It.IsAny<IEnumerable<ClientScript>>()
                )
            );
            actual.Should().BeEquivalentTo(
                expectedList
            );
        }

        [Fact]
        public async Task ShouldNotSetHashAndScriptAssemblyWhenComplierSuccessIsFalse()
        {
            // Given
            var compileResult = new CompiledScriptResult(
                "error-code"
            );
            var repositoryAll = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    "path",
                    "file-name",
                    "script-string"
                ),
            };

            var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var clientScriptsStateMock = new Mock<ClientScriptsState>();
            var clientScriptRepositoryMock = new Mock<ClientScriptRepository>();
            var clientScriptCompilerMock = new Mock<ClientScriptCompiler>();

            clientScriptRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                repositoryAll
            );

            clientScriptCompilerMock.Setup(
                mock => mock.Compile(
                    It.IsAny<IEnumerable<ClientScript>>()
                )
            ).ReturnsAsync(
                compileResult
            );

            // When
            var handler = new CompileClientScriptCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                clientScriptsStateMock.Object,
                clientScriptRepositoryMock.Object,
                clientScriptCompilerMock.Object
            );
            await handler.Handle(
                new CompileClientScriptCommand(),
                CancellationToken.None
            );


            // Then
            clientScriptsStateMock.Verify(
                mock => mock.SetAssembly(
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldSetHashAndScriptAssemblyWhenComplierSuccessIsTrue()
        {
            // Given
            var expectedHash = "hash";
            var expectedScriptAssembly = "script-assembly";
            var compileResult = new CompiledScriptResult(
                expectedHash,
                expectedScriptAssembly
            );
            var repositoryAll = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    "path",
                    "file-name",
                    "script-string"
                ),
            };

            var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var clientScriptsStateMock = new Mock<ClientScriptsState>();
            var clientScriptRepositoryMock = new Mock<ClientScriptRepository>();
            var clientScriptCompilerMock = new Mock<ClientScriptCompiler>();

            clientScriptRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                repositoryAll
            );

            clientScriptCompilerMock.Setup(
                mock => mock.Compile(
                    It.IsAny<IEnumerable<ClientScript>>()
                )
            ).ReturnsAsync(
                compileResult
            );

            // When
            var handler = new CompileClientScriptCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                clientScriptsStateMock.Object,
                clientScriptRepositoryMock.Object,
                clientScriptCompilerMock.Object
            );
            await handler.Handle(
                new CompileClientScriptCommand(),
                CancellationToken.None
            );

            // Then
            clientScriptsStateMock.Verify(
                mock => mock.SetAssembly(
                    expectedHash,
                    expectedScriptAssembly
                )
            );
        }

        [Fact]
        public async Task ShouldPublishClientActionScriptAssemblyChangedEventWhenComplierSuccessIsTrue()
        {
            // Given
            var hash = "hash";
            var scriptAssembly = "script-assembly";
            var compileResult = new CompiledScriptResult(
                hash,
                scriptAssembly
            );
            var repositoryAll = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    "path",
                    "file-name",
                    "script-string"
                ),
            };
            var expectedAction = "CLIENT_SCRIPTS_ASSEMBLY_CHANGED_CLIENT_ACTION_EVENT";
            var expectedHash = hash;
            var expectedScriptAssembly = scriptAssembly;

            var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var clientScriptsStateMock = new Mock<ClientScriptsState>();
            var clientScriptRepositoryMock = new Mock<ClientScriptRepository>();
            var clientScriptCompilerMock = new Mock<ClientScriptCompiler>();

            clientScriptRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                repositoryAll
            );

            clientScriptCompilerMock.Setup(
                mock => mock.Compile(
                    It.IsAny<IEnumerable<ClientScript>>()
                )
            ).ReturnsAsync(
                compileResult
            );

            // When
            var handler = new CompileClientScriptCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                clientScriptsStateMock.Object,
                clientScriptRepositoryMock.Object,
                clientScriptCompilerMock.Object
            );
            var actual = default(ClientActionGenericToAllEvent);

            mediatorMock.Setup(
                mock => mock.Publish(
                    It.IsAny<ClientActionGenericToAllEvent>(),
                    CancellationToken.None
                )
            ).Callback<ClientActionGenericToAllEvent, CancellationToken>(
                (notification, _) =>
                {
                    actual = notification;
                }
            );
            await handler.Handle(
                new CompileClientScriptCommand(),
                CancellationToken.None
            );

            // Then
            actual.Action.Should().Be(
                expectedAction
            );
            actual.Data.Should().BeOfType(
                typeof(ClientScriptsAssemblyChangedClientActionData)
            );

            var actionData = (ClientScriptsAssemblyChangedClientActionData)actual.Data;
            actionData.Hash.Should().Be(
                expectedHash
            );
            actionData.ScriptAssembly.Should().Be(
                expectedScriptAssembly
            );
        }
    }
}
