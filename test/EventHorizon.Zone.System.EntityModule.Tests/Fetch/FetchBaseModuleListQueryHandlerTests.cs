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

public class FetchBaseModuleListQueryHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ReturnTheListOfAllBaseModulesWhenRequestIsHandled(
        // Given
        List<EntityScriptModule> entityScriptModules,
        [Frozen] Mock<EntityModuleRepository> entityModuleRepositoryMock,
        FetchBaseModuleListQueryHandler handle
    )
    {
        entityModuleRepositoryMock.Setup(
            mock => mock.ListOfAllBaseModules()
        ).Returns(
            entityScriptModules
        );

        // When
        var actual = await handle.Handle(
            new FetchBaseModuleListQuery(),
            CancellationToken.None
        );

        // Then
        actual.Should().BeEquivalentTo(entityScriptModules);
    }
}
