namespace EventHorizon.Zone.System.Server.Scripts.Tests.State;

using EventHorizon.Zone.System.Server.Scripts.Exceptions;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.State;

using FluentAssertions;

using global::System;
using global::System.Collections.Generic;
using global::System.Threading.Tasks;

using Xunit;

public class ServerScriptInMemoryRepositoryTests
{
    [Fact]
    public void TestShouldBeFindScriptBasedOnIdWhenAddedToRepository()
    {
        // Given
        var scriptId = "script-id";
        var serverScript = new TestServerScript
        {
            Id = scriptId
        };
        var expected = serverScript;

        // When
        var repository = new ServerScriptInMemoryRepository();

        repository.Add(
            serverScript
        );

        var actual = repository.Find(
            scriptId
        );

        // Then
        Assert.Equal(
            expected,
            actual
        );
    }

    [Fact]
    public void TestShouldThrowNotFoundExceptionWhenFindScriptIsCalledWithScriptId()
    {
        // Given
        var scriptId = "script-id";
        var expected = "ServerScriptInMemoryRepository did not find the script.";
        var expectedScriptId = scriptId;

        // When
        var repository = new ServerScriptInMemoryRepository();
        ServerScript action() => repository.Find(
            scriptId
        );

        // Then
        var actualException = Assert.Throws<ServerScriptNotFound>(
            action
        );

        actualException.Message
            .Should().Be(expected);
        actualException.ScriptId
            .Should().Be(expectedScriptId);
    }

    [Fact]
    public void ShouldReturnEmptyListForALlWhenClearIsCalled()
    {
        // Given
        var serverScript = new TestServerScript
        {
            Id = "script-id",
        };

        // When
        var repository = new ServerScriptInMemoryRepository();
        repository.Add(
            serverScript
        );
        repository.All
            .Should().HaveCount(1);
        repository.Clear();

        // Then
        repository.All
            .Should().BeEmpty();
    }

    [Fact]
    public void ShouldReplaceExistingScriptWhenAddScriptHasSameIdAsAlreadyExisting()
    {
        // Given
        var serverScript = new TestServerScript
        {
            Id = "script-id",
            Tags = new List<string> { "tag1" },
        };
        var server2Script = new TestServerScript
        {
            Id = "script-id",
            Tags = new List<string> { "tag2" },
        };

        var expected = new List<ServerScript>
        {
            server2Script,
        };

        // When
        var repository = new ServerScriptInMemoryRepository();
        repository.Add(
            serverScript
        );
        repository.Add(
            server2Script
        );

        // Then
        repository.All
            .Should().BeEquivalentTo(expected);
    }

    public struct TestServerScript
        : ServerScript
    {
        public string Id { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public Task<ServerScriptResponse> Run(
            ServerScriptServices services,
            IDictionary<string, object> data
        )
        {
            throw new global::System.NotImplementedException();
        }

        public Task<ServerScriptResponse> Run(ServerScriptServices services, ServerScriptData data)
        {
            throw new NotImplementedException();
        }
    }
}
