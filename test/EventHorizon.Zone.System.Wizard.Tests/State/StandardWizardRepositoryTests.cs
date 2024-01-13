namespace EventHorizon.Zone.System.Wizard.Tests.State;

using EventHorizon.Zone.System.Wizard.Model;
using EventHorizon.Zone.System.Wizard.State;

using FluentAssertions;

using global::System.Collections.Generic;

using Xunit;

public class StandardWizardRepositoryTests
{
    [Fact]
    public void ShouldReturnAllWizardsWhenSetInRepository()
    {
        // Given
        var wizardId = "wizard-1";
        var wizard = new WizardMetadata
        {
            Id = wizardId,
        };

        var expected = new List<WizardMetadata>
        {
            wizard,
        };

        // When
        var repository = new StandardWizardRepository();
        repository.Set(
            wizardId,
            wizard
        );
        var actual = repository.All;

        // Then
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ShouldReturnZeroWizardsWhenSetInRepositoryAndThenCleared()
    {
        // Given
        var wizardId = "wizard-1";
        var wizard = new WizardMetadata
        {
            Id = wizardId,
        };

        // When
        var repository = new StandardWizardRepository();
        repository.Set(
            wizardId,
            wizard
        );

        repository.All.Should().NotBeEmpty();
        repository.Clear();

        var actual = repository.All;

        // Then
        actual.Should().BeEmpty();
    }

    [Fact]
    public void ShouldOverrideExistingWizardWhenSetIsCalledWithSameWizardIdAndDifferentWizard()
    {
        // Given
        var wizardId = "wizard-1";
        var wizard1 = new WizardMetadata
        {
            Id = wizardId,
            Name = "Fist Set Wizard",
        };
        var wizard2 = new WizardMetadata
        {
            Id = wizardId,
            Name = "Second Set Wizard",
        };

        // When
        var repository = new StandardWizardRepository();
        repository.Set(
            wizardId,
            wizard1
        );

        repository.All.Should().Contain(wizard1)
            .And
            .Subject.Should().NotContain(wizard2);

        repository.Set(
            wizardId,
            wizard2
        );

        // Then
        repository.All.Should().Contain(wizard2)
            .And
            .Subject.Should().NotContain(wizard1);
    }

    [Fact]
    public void ShouldReturnEmptyOptionWhenWizardIsNotInRepository()
    {
        // Given
        var wizardId = "wizard-1";

        // When
        var repository = new StandardWizardRepository();
        var actual = repository.Get(
            wizardId
        );

        // Then
        actual.HasValue
            .Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnWizardInOptionWhenRepositoryContainsWizard()
    {
        // Given
        var wizardId = "wizard-1";
        var wizard = new WizardMetadata
        {
            Id = wizardId,
            Name = "Fist Set Wizard",
        };

        // When
        var repository = new StandardWizardRepository();
        repository.Set(
            wizardId,
            wizard
        );

        var actual = repository.Get(
            wizardId
        );

        // Then
        actual.HasValue
            .Should().BeTrue();
        actual.Value
            .Should().Be(wizard);
    }
}
