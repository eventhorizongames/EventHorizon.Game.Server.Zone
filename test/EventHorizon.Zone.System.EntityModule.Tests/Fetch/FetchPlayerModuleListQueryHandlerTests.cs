namespace EventHorizon.Zone.System.EntityModule.Tests.Fetch;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Fetch;
using EventHorizon.Zone.System.EntityModule.Model;

using FluentAssertions;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;

public class FetchPlayerModuleListQueryHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ReturnTheListOfAllPlayerModulesWhenRequestIsHandled(
        // Given
        List<EntityScriptModule> entityScriptModules,
        [Frozen] Mock<EntityModuleRepository> entityModuleRepositoryMock,
        FetchPlayerModuleListQueryHandler handle
    )
    {
        entityModuleRepositoryMock.Setup(
            mock => mock.ListOfAllPlayerModules()
        ).Returns(
            entityScriptModules
        );

        // When
        var actual = await handle.Handle(
            new FetchPlayerModuleListQuery(),
            CancellationToken.None
        );

        // Then
        actual.Should().BeEquivalentTo(entityScriptModules);
    }
}
