namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Tests.CSharp
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.CSharp;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ClientScriptCompilerForCSharpTests
    {
        [Fact]
        public async Task ShouldReturnHashAndEncodedFileContentFromAssemblyEvaluator()
        {
            // Given
            var scriptContent = "script-assembly";
            var scriptAssembly = "script-assembly";
            var nl = Environment.NewLine;
            var compiledSoruce = $"                        {nl}{nl}{nl}script-assembly{nl}";
            var generatedFileFullName = "generated-file-full-name";
            var scriptAssemblyBytes = scriptAssembly.ToBytes();

            var expected = "c2NyaXB0LWFzc2VtYmx5";

            var scripts = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    "ScriptName",
                    "script-path",
                    scriptContent
                )
            };

            var loggerMock = new Mock<ILogger<ClientScriptCompilerForCSharp>>();
            var mediatorMock = new Mock<IMediator>();
            var assemblyBuilderMock = new Mock<AssemblyBuilder>();
            var clientScriptsConsolidatorMock = new Mock<ClientScriptsConsolidator>();

            clientScriptsConsolidatorMock.Setup(
                mock => mock.IntoSingleTemplatedString(
                    scripts,
                    ref It.Ref<List<string>>.IsAny
                )
            ).Returns(
                scriptAssembly
            );

            assemblyBuilderMock.Setup(
                mock => mock.Compile(
                    compiledSoruce
                )
            ).ReturnsAsync(
                generatedFileFullName
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextAsBytesFromFile(
                        generatedFileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                scriptAssemblyBytes
            );

            // When
            var compiler = new ClientScriptCompilerForCSharp(
                loggerMock.Object,
                mediatorMock.Object,
                assemblyBuilderMock.Object,
                clientScriptsConsolidatorMock.Object
            );
            var actual = await compiler.Compile(
                scripts
            );

            // Then
            actual.Hash.Should().NotBeNullOrEmpty();
            actual.ScriptAssembly.Should().Be(
                expected
            );
        }

        [Fact]
        public async Task ShouldReturnErrorCodeResultOnAnyException()
        {
            // Given
            var expected = new CompiledScriptResult(
                "csharp_failed_to_compile"
            );

            var loggerMock = new Mock<ILogger<ClientScriptCompilerForCSharp>>();
            var mediatorMock = new Mock<IMediator>();
            var assemblyBuilderMock = new Mock<AssemblyBuilder>();
            var clientScriptsConsolidatorMock = new Mock<ClientScriptsConsolidator>();

            clientScriptsConsolidatorMock.Setup(
                mock => mock.IntoSingleTemplatedString(
                    It.IsAny<IEnumerable<ClientScript>>(),
                    ref It.Ref<List<string>>.IsAny
                )
            ).Throws(
                new Exception("error")
            );

            // When
            var compiler = new ClientScriptCompilerForCSharp(
                loggerMock.Object,
                mediatorMock.Object,
                assemblyBuilderMock.Object,
                clientScriptsConsolidatorMock.Object
            );
            var actual = await compiler.Compile(
                new List<ClientScript>()
            );


            // Then
            actual.Should().Be(
                expected
            );
        }
    }
}
