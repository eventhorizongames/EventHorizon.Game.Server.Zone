using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Map.State;
using System.Numerics;
using EventHorizon.Game.Server.Zone.State.Impl;

namespace EventHorizon.Game.Server.Zone.Tests.State.Impl
{
    public class ServerStateTests
    {
        [Fact]
        public async Task TestSetMap_ShouldSetMapGraphInToServerState()
        {
            // Given
            var expected = new MapGraph(Vector3.One, Vector3.One, false);

            // When
            var serverState = new ServerState();

            await serverState.SetMap(expected);

            var actual = await serverState.Map();
            
            // Then
            Assert.Equal(expected, actual);
        }
    }
}