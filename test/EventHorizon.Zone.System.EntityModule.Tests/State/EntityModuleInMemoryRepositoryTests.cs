namespace EventHorizon.Zone.System.EntityModule.Tests.State;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.EntityModule.Model;
using EventHorizon.Zone.System.EntityModule.State;

using FluentAssertions;

using Xunit;

public class EntityModuleInMemoryRepositoryTests
{
    [Theory, AutoMoqData]
    public void Remove_All_Base_And_Player_Module_Scripts_When_Clear_Is_Triggered(
        // Given
        EntityScriptModule baseModule,
        EntityScriptModule playerModule,
        EntityModuleInMemoryRepository repository
    )
    {
        // When
        repository.ListOfAllBaseModules()
            .Should().BeEmpty();
        repository.ListOfAllPlayerModules()
            .Should().BeEmpty();

        repository.AddBaseModule(
            baseModule
        );
        repository.AddPlayerModule(
            playerModule
        );

        repository.ListOfAllBaseModules()
            .Should().BeEquivalentTo(new[] { baseModule });
        repository.ListOfAllPlayerModules()
            .Should().BeEquivalentTo(new[] { playerModule });

        repository.Clear();

        // Then
        repository.ListOfAllBaseModules()
            .Should().BeEmpty();
        repository.ListOfAllPlayerModules()
            .Should().BeEmpty();
    }

    [Theory, AutoMoqData]
    public void Does_Not_Add_Duplicate_Base_Modules_When_Name_Of_Module_Already_Exists(
        // Given
        EntityScriptModule baseModule,
        EntityModuleInMemoryRepository repository
    )
    {
        // When
        repository.AddBaseModule(
            baseModule
        );
        repository.AddBaseModule(
            baseModule
        );

        // Then
        repository.ListOfAllBaseModules()
            .Should().BeEquivalentTo(new[] { baseModule });
    }

    [Theory, AutoMoqData]
    public void Does_Not_Add_Duplicate_Player_Modules_When_Name_Of_Module_Already_Exists(
        // Given
        EntityScriptModule playerModule,
        EntityModuleInMemoryRepository repository
    )
    {
        // When
        repository.AddPlayerModule(
            playerModule
        );
        repository.AddPlayerModule(
            playerModule
        );

        // Then
        repository.ListOfAllPlayerModules()
            .Should().BeEquivalentTo(new[] { playerModule });
    }
}
