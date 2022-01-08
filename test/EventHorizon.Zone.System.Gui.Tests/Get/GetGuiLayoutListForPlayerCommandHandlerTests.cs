namespace EventHorizon.Zone.System.Gui.Tests.Get;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Gui.Events.Layout;
using EventHorizon.Zone.System.Gui.Get;
using EventHorizon.Zone.System.Gui.Model;

using FluentAssertions;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using Xunit;


public class GetGuiLayoutListForPlayerCommandHandlerTests
{
    [Theory, AutoMoqData(disableRecursionCheck: true)]
    public async Task ReturnEnumerableListOfGuiLayoutsWhenQueryIsHandled(
        // Given
        [Frozen] IEnumerable<GuiLayout> layoutList,
        GetGuiLayoutListForPlayerCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            new GetGuiLayoutListForPlayerCommand(),
            CancellationToken.None
        );

        // Then
        actual.Should().BeEquivalentTo(
            layoutList
        );
    }
}
