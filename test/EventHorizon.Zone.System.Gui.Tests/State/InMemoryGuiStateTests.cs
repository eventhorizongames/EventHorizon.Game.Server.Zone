namespace EventHorizon.Zone.System.Gui.Tests.State;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Gui.Model;
using EventHorizon.Zone.System.Gui.State;

using FluentAssertions;

using global::System.Collections.Generic;

using Xunit;


public class InMemoryGuiStateTests
{
    [Theory, AutoMoqData(disableRecursionCheck: true)]
    public void ReturnAllAddedGuiLayoutsByIdWhenMultipleLayoutsAreAdded(
        // Given
        GuiLayout layout1,
        GuiLayout layout2,
        GuiLayout layout3,
        InMemoryGuiState state
    )
    {
        // When
        state.AddLayout(
            layout1.Id,
            layout1
        );
        state.AddLayout(
            layout2.Id,
            layout2
        );
        state.AddLayout(
            layout3.Id,
            layout3
        );
        var actual = state.All();

        // Then
        actual.Should().BeEquivalentTo(
            new List<GuiLayout>
            {
                layout1,
                layout2,
                layout3,
            }
        );
    }

    [Theory, AutoMoqData(disableRecursionCheck: true)]
    public void OnlyReturnLastAddedLayoutWhenIdsAreTheSame(
        // Given
        string id,
        GuiLayout layout1,
        GuiLayout layout2,
        GuiLayout layout3,
        InMemoryGuiState state
    )
    {
        // When
        state.AddLayout(
            id,
            layout1
        );
        state.AddLayout(
            id,
            layout2
        );
        state.AddLayout(
            id,
            layout3
        );
        var actual = state.All();

        // Then
        actual.Should().BeEquivalentTo(
            new List<GuiLayout>
            {
                layout3,
            }
        );
    }

    [Theory, AutoMoqData(disableRecursionCheck: true)]
    public void ClearAllAddedLayoutsWhenClearingState(
        // Given
        GuiLayout layout1,
        GuiLayout layout2,
        GuiLayout layout3,
        InMemoryGuiState state
    )
    {
        // When
        state.AddLayout(
            layout1.Id,
            layout1
        );
        state.AddLayout(
            layout2.Id,
            layout2
        );
        state.AddLayout(
            layout3.Id,
            layout3
        );
        var actual = state.All();
        actual.Should().HaveCount(3);
        state.Clear();

        actual = state.All();

        // Then
        actual.Should().BeEmpty();
    }
}
