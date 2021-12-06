namespace EventHorizon.Zone.Core.Tests.DirectoryService;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.DirectoryService;
using EventHorizon.Zone.Core.Events.DirectoryService;

using FluentAssertions;

using Xunit;

public class DeleteDirectoryRecursivelyCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ReturnsSuccessWhenCommandIsHandled(
        // Given
        DeleteDirectoryRecursivelyCommand command,
        DeleteDirectoryRecursivelyCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            command,
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
    }
}
