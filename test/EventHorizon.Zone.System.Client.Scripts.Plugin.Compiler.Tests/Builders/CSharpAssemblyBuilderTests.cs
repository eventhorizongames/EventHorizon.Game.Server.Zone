namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Tests.Builders
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Builders;

    using FluentAssertions;

    using global::System;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class CSharpAssemblyBuilderTests
    {
        string _fileTempDirectory = string.Empty;

        public CSharpAssemblyBuilderTests()
        {
            // Cleanup _fileTempDirectory used during testing
            _fileTempDirectory = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Builders",
                ".temp"
            );
            if (Directory.Exists(
                _fileTempDirectory
            ))
            {
                Directory.Delete(
                    _fileTempDirectory,
                    true
                );
            }
            Directory.CreateDirectory(
                _fileTempDirectory
            );
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldReturnPathOfGeneratedLibraryWhenAssemblyIsCompiled()
        {
            // Given
            var assemblyAsString = "System.Console.WriteLine(\"hi\");";
            var generatedPath = "_generated";
            var expected = Path.Combine(
                generatedPath,
                "Scripts.dll"
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        generatedPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            serverInfoMock.Setup(
                mock => mock.FileSystemTempPath
            ).Returns(
                _fileTempDirectory
            );
            serverInfoMock.Setup(
                mock => mock.GeneratedPath
            ).Returns(
                generatedPath
            );

            // When
            var builder = new CSharpAssemblyBuilder(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await builder.Compile(
                assemblyAsString
            );

            // Then
            actual.Should().Be(
                expected
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new CreateDirectory(
                        generatedPath
                    ),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldCreateDirectoryToGeneratedPathWhenDoesNotExist()
        {
            // Given
            var assemblyAsString = "System.Console.WriteLine(\"hi\");";
            var generatedPath = "_generated";
            var expected = Path.Combine(
                generatedPath,
                "Scripts.dll"
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        generatedPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                false
            );

            serverInfoMock.Setup(
                mock => mock.FileSystemTempPath
            ).Returns(
                _fileTempDirectory
            );
            serverInfoMock.Setup(
                mock => mock.GeneratedPath
            ).Returns(
                generatedPath
            );

            // When
            var builder = new CSharpAssemblyBuilder(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await builder.Compile(
                assemblyAsString
            );

            // Then
            actual.Should().Be(
                expected
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new CreateDirectory(
                        generatedPath
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldAllowForRegisteringAssembliesWhenAddReferenceAssemblyIsCalled()
        {
            // Given
            var assemblyAsString = "System.Console.WriteLine(\"hi\");";
            var generatedPath = "_generated";
            var expected = Path.Combine(
                generatedPath,
                "Scripts.dll"
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        generatedPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                false
            );

            serverInfoMock.Setup(
                mock => mock.FileSystemTempPath
            ).Returns(
                _fileTempDirectory
            );
            serverInfoMock.Setup(
                mock => mock.GeneratedPath
            ).Returns(
                generatedPath
            );

            // When
            var builder = new CSharpAssemblyBuilder(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            builder.ReferenceAssembly(
                typeof(CSharpAssemblyBuilder).Assembly
            );
            var actual = await builder.Compile(
                assemblyAsString
            );

            // Then
            actual.Should().Be(
                expected
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new CreateDirectory(
                        generatedPath
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
