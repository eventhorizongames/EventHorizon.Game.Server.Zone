namespace EventHorizon.Zone.System.Backup.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Test.Common.Utils;
    using FluentAssertions;
    using MediatR;
    using Moq;
    using Xunit;

    public class SystemBackupExtensionsTests
    {
        [Theory, AutoMoqData]
        public void TestAddServerSetup_ShouldAddExpectedServices(
            // Given
            ServiceCollectionMock serviceCollectionMock
        )
        {
            // When
            var actual = SystemBackupExtensions.AddSystemBackup(
                serviceCollectionMock
            );

            // Then
            actual.Should().BeEquivalentTo(
                serviceCollectionMock
            );
        }

        [Fact]
        public void TestShouldReturnApplicationBuilderForChainingCommands()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();

            // When
            var actual = SystemBackupExtensions.UseSystemBackup(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            actual.Should().Be(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );
        }
    }
}
